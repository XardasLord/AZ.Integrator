# Import definicji mebla z pliku Excel

## Opis funkcjonalności

Funkcjonalność importu z Excel pozwala na szybkie wypełnienie formularza definicji mebla danymi z pliku Excel (.xlsx, .xls).

## Format pliku Excel

### Struktura arkusza

Plik Excel musi mieć dokładnie następującą strukturę:

**Wiersz 1 - Nagłówki kolumn:**
- Kolumna A: `kod_mebla`
- Kolumna B: `nazwa_formatki`
- Kolumna C: `dlugosc_mm`
- Kolumna D: `szerokosc_mm`
- Kolumna E: `grubosc_mm`
- Kolumna F: `ilosc`
- Kolumna G: `okleina_dlugosc_krawedzie`
- Kolumna H: `okleina_szerokosc_krawedzie`
- Kolumna I: `dodatkowe_informacje`

**Wiersz 2 i następne - Dane formatek:**
- Każdy wiersz reprezentuje jedną formatkę
- Kolumna A (kod_mebla) powinna zawierać ten sam kod dla wszystkich wierszy
- Puste wiersze są automatycznie pomijane

### Wymagane kolumny

| Kolumna | Nazwa | Typ | Opis |
|---------|-------|-----|------|
| A | kod_mebla | Tekst | Kod mebla (wymagane) |
| B | nazwa_formatki | Tekst | Nazwa formatki (wymagane) |
| C | dlugosc_mm | Liczba | Długość formatki w milimetrach (wymagane) |
| D | szerokosc_mm | Liczba | Szerokość formatki w milimetrach (wymagane) |
| E | grubosc_mm | Liczba | Grubość formatki w milimetrach (wymagane) |

### Opcjonalne kolumny

| Kolumna | Nazwa | Typ | Domyślna wartość | Opis |
|---------|-------|-----|------------------|------|
| F | ilosc | Liczba | 1 | Ilość sztuk formatki |
| G | okleina_dlugosc_krawedzie | Liczba/Tekst | 0 (brak) | Oklejenie krawędzi wzdłuż długości |
| H | okleina_szerokosc_krawedzie | Liczba/Tekst | 0 (brak) | Oklejenie krawędzi wzdłuż szerokości |
| I | dodatkowe_informacje | Tekst | "" | Dodatkowe informacje o formatce |

### Wartości oklejenia krawędzi

Oklejenie można podać jako:
- **Liczba**: `0` (brak), `1` (jedna krawędź), `2` (dwie krawędzie)
- **Tekst**: `"brak"`, `"jedna"`, `"dwie"`, `"nie"`, `"tak"`, itp.

## Przykładowy format arkusza

```
A              | B              | C    | D   | E  | F | G | H | I
---------------|----------------|------|-----|----|---|---|---|--------
kod_mebla      | nazwa_formatki | dlugosc_mm | szerokosc_mm | grubosc_mm | ilosc | okleina_dlugosc_krawedzie | okleina_szerokosc_krawedzie | dodatkowe_informacje
SZAFA_AGA_80   | Bok prawy      | 2000 | 600 | 18 | 1 | 2 | 1 | 
SZAFA_AGA_80   | Bok lewy       | 2000 | 600 | 18 | 1 | 2 | 1 |
SZAFA_AGA_80   | Półka górna    | 800  | 600 | 18 | 1 | 1 | 1 |
SZAFA_AGA_80   | Półka środkowa | 800  | 600 | 18 | 2 | 1 | 1 | Dwie sztuki
SZAFA_AGA_80   | Tył            | 1964 | 796 | 5  | 1 | 0 | 0 | HDF
```

## Ważne uwagi

1. **Kod mebla**: Wartość z kolumny A w wierszu 2 będzie użyta jako kod mebla dla całej definicji
2. **Jeden plik = jeden mebel**: Wszystkie wiersze powinny mieć ten sam kod_mebla w kolumnie A
3. **Wielkość liter**: Nazwy kolumn mogą być pisane małymi lub wielkimi literami (kod rozpozna oba)
4. **Separatory**: W nazwach kolumn można używać podkreślników lub ich nie używać (np. `dlugosc_mm` lub `dlugoscmm`)

## Jak używać

1. Otwórz formularz dodawania/edycji definicji mebla
2. Kliknij przycisk **"Import z Excel"** obok pola "Kod mebla"
3. Wybierz plik Excel (.xlsx lub .xls)
4. System automatycznie:
   - Wypełni kod mebla wartością z kolumny A wiersza 2 (jeśli nie jesteś w trybie edycji)
   - Usunie wszystkie istniejące formatki
   - Doda wszystkie formatki z pliku Excel
5. Sprawdź zaimportowane dane i zapisz definicję

## Obsługa błędów

System automatycznie wykrywa i raportuje następujące problemy:
- Brak wymaganych kolumn w arkuszu
- Brak kodu mebla (kolumna A, wiersz 2)
- Nieprawidłowe wartości liczbowe
- Puste lub nieprawidłowe wiersze (są pomijane)
- Błędy parsowania pliku Excel

W przypadku błędu, zostanie wyświetlony komunikat z opisem problemu.

## Wskazówki

- Upewnij się, że wszystkie wymiary są w milimetrach
- Używaj kropki lub przecinka jako separatora dziesiętnego
- Puste wiersze są automatycznie pomijane
- Wielkość liter w nagłówkach nie ma znaczenia
- Podkreślniki w nazwach kolumn są opcjonalne

