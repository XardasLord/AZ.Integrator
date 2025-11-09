import { Injectable } from '@angular/core';
import * as XLSX from 'xlsx';
import { FurnitureImportData, PartImportData } from '../models/furniture-import.model';

interface ColumnMap {
  name: number;
  lengthMm: number;
  widthMm: number;
  thicknessMm: number;
  lengthEdgeBanding: number;
  widthEdgeBanding: number;
  quantity: number;
  additionalInfo: number;
}

@Injectable({
  providedIn: 'root',
})
export class ExcelImportService {
  /**
   * Importuje dane definicji mebla z pliku Excel
   * @param file Plik Excel do zaimportowania
   * @returns Promise z danymi zaimportowanego mebla
   */
  async importFurnitureFromExcel(file: File): Promise<FurnitureImportData> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();

      reader.onload = (e: ProgressEvent<FileReader>) => {
        try {
          const data = new Uint8Array(e.target!.result as ArrayBuffer);
          const workbook = XLSX.read(data, { type: 'array' });

          // Pobierz pierwszy arkusz
          const firstSheetName = workbook.SheetNames[0];
          const worksheet = workbook.Sheets[firstSheetName];

          // Konwertuj arkusz na JSON
          const jsonData: unknown[][] = XLSX.utils.sheet_to_json(worksheet, { header: 1 });

          // Parsuj dane
          const furnitureData = this.parseExcelData(jsonData);
          resolve(furnitureData);
        } catch (error) {
          reject(new Error('Błąd podczas parsowania pliku Excel: ' + error));
        }
      };

      reader.onerror = () => {
        reject(new Error('Błąd podczas odczytu pliku'));
      };

      reader.readAsArrayBuffer(file);
    });
  }

  /**
   * Parsuje dane z arkusza Excel do struktury FurnitureImportData
   * Oczekiwany format:
   * - Wiersz 1: Nagłówki (kod_mebla, nazwa_formatki, dlugosc_mm, etc.)
   * - Wiersz 2+: Dane formatek
   */
  private parseExcelData(data: unknown[][]): FurnitureImportData {
    if (!data || data.length < 2) {
      throw new Error('Plik Excel jest pusty lub ma nieprawidłowy format');
    }

    // Wiersz 1 to nagłówki
    const headerRowIndex = 0;
    const headers = data[headerRowIndex].map((h: unknown) => (h ? h.toString().toLowerCase().trim() : ''));

    // Mapuj nagłówki na indeksy kolumn
    const columnMap = this.mapColumnHeaders(headers);

    // Wyciągnij kod mebla z pierwszego wiersza danych (kolumna A)
    let furnitureCode = '';
    if (data.length > 1 && data[1] && data[1][0]) {
      furnitureCode = data[1][0].toString().trim();
    }

    if (!furnitureCode) {
      throw new Error('Nie znaleziono kodu mebla w pliku Excel (kolumna A, wiersz 2)');
    }

    // Parsuj formatki (zaczynając od wiersza 2)
    const parts: PartImportData[] = [];
    for (let i = 1; i < data.length; i++) {
      const row = data[i];

      // Pomiń puste wiersze
      if (!row || row.length === 0 || !row[columnMap.name]) {
        continue;
      }

      try {
        const part = this.parsePartRow(row, columnMap);
        if (part) {
          parts.push(part);
        }
      } catch (error) {
        console.warn(`Pominięto wiersz ${i + 1}:`, error);
      }
    }

    if (parts.length === 0) {
      throw new Error('Nie znaleziono żadnych formatek w pliku Excel');
    }

    return {
      furnitureCode,
      parts,
    };
  }

  /**
   * Mapuje nagłówki kolumn na ich indeksy
   * Oczekiwane nazwy kolumn (wielkość liter nie ma znaczenia):
   * - kod_mebla (kolumna A)
   * - nazwa_formatki (kolumna B)
   * - dlugosc_mm (kolumna C)
   * - szerokosc_mm (kolumna D)
   * - grubosc_mm (kolumna E)
   * - ilosc (kolumna F)
   * - okleina_dlugosc_krawedzie (kolumna G)
   * - okleina_szerokosc_krawedzie (kolumna H)
   * - dodatkowe_informacje (kolumna I)
   */
  private mapColumnHeaders(headers: string[]): ColumnMap {
    const columnMap: ColumnMap = {
      name: -1,
      lengthMm: -1,
      widthMm: -1,
      thicknessMm: -1,
      lengthEdgeBanding: -1,
      widthEdgeBanding: -1,
      quantity: -1,
      additionalInfo: -1,
    };

    headers.forEach((header, index) => {
      const h = header.toLowerCase().replace(/_/g, '');

      // Dokładne mapowanie kolumn zgodnie ze strukturą Excel
      if (h.includes('nazwaformatki')) {
        columnMap.name = index;
      } else if (h.includes('dlugoscmm')) {
        columnMap.lengthMm = index;
      } else if (h.includes('szerokoscmm')) {
        columnMap.widthMm = index;
      } else if (h.includes('gruboscmm')) {
        columnMap.thicknessMm = index;
      } else if (h.includes('okleinadlugosckrawedzie')) {
        columnMap.lengthEdgeBanding = index;
      } else if (h.includes('okleinaszerokosckrawedzie')) {
        columnMap.widthEdgeBanding = index;
      } else if (h.includes('ilosc')) {
        columnMap.quantity = index;
      } else if (h.includes('dodatkoweinformacje')) {
        columnMap.additionalInfo = index;
      }
    });

    // Walidacja - wymagane kolumny
    const requiredColumns: (keyof ColumnMap)[] = ['name', 'lengthMm', 'widthMm', 'thicknessMm', 'quantity'];
    for (const col of requiredColumns) {
      if (columnMap[col] === -1) {
        throw new Error(`Nie znaleziono wymaganej kolumny: ${col}`);
      }
    }

    return columnMap;
  }

  /**
   * Parsuje pojedynczy wiersz z formatką
   */
  private parsePartRow(row: unknown[], columnMap: ColumnMap): PartImportData | null {
    const name = row[columnMap.name]?.toString().trim();
    if (!name) {
      return null;
    }

    const lengthMm = this.parseNumber(row[columnMap.lengthMm]);
    const widthMm = this.parseNumber(row[columnMap.widthMm]);
    const thicknessMm = this.parseNumber(row[columnMap.thicknessMm]);

    if (!lengthMm || !widthMm || !thicknessMm) {
      throw new Error(`Nieprawidłowe wymiary dla formatki: ${name}`);
    }

    const lengthEdgeBandingType = this.parseEdgeBanding(row[columnMap.lengthEdgeBanding]);
    const widthEdgeBandingType = this.parseEdgeBanding(row[columnMap.widthEdgeBanding]);

    const quantity = columnMap.quantity !== -1 ? this.parseNumber(row[columnMap.quantity]) || 1 : 1;

    const additionalInfo =
      columnMap.additionalInfo !== -1 ? row[columnMap.additionalInfo]?.toString().trim() || '' : '';

    return {
      name,
      lengthMm,
      widthMm,
      thicknessMm,
      lengthEdgeBandingType,
      widthEdgeBandingType,
      quantity,
      additionalInfo,
    };
  }

  /**
   * Parsuje wartość numeryczną z komórki Excel
   */
  private parseNumber(value: unknown): number | null {
    if (value === null || value === undefined || value === '') {
      return null;
    }

    const num = typeof value === 'number' ? value : parseFloat(value.toString().replace(',', '.'));
    return isNaN(num) ? null : num;
  }

  /**
   * Parsuje typ oklejenia krawędzi
   * 0 = brak, 1 = jedna krawędź, 2 = dwie krawędzie
   */
  private parseEdgeBanding(value: unknown): number {
    if (value === null || value === undefined || value === '') {
      return 0;
    }

    const str = value.toString().toLowerCase().trim();

    // Jeśli to liczba
    const num = parseInt(str);
    if (!isNaN(num)) {
      return Math.min(Math.max(num, 0), 2);
    }

    // Jeśli to tekst
    if (str.includes('brak') || str === '0' || str === 'nie' || str === 'no') {
      return 0;
    } else if (str.includes('jedna') || str === '1' || str.includes('one')) {
      return 1;
    } else if (str.includes('dwie') || str.includes('dwa') || str === '2' || str.includes('two')) {
      return 2;
    }

    return 0; // Domyślnie brak
  }
}
