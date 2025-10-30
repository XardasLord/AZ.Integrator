# Moduł zarządzania dostawcami (Suppliers)

## Podsumowanie

Został utworzony pełny moduł zarządzania dostawcami w `furniture-formats/suppliers` zgodnie z architekturą i stylem modułu `furniture-formats/formats`.

## Utworzone pliki

### 1. GraphQL Queries
- **get-suppliers.graphql.query.ts** - Zapytanie GraphQL do pobierania dostawców z paginacją

### 2. Models
- **commands/save-supplier.command.ts** - Command do zapisu/aktualizacji dostawcy
- **supplier-form-group.model.ts** - Model formularza reaktywnego dla dostawcy
- **supplier-mailbox-form-group.model.ts** - Model formularza dla adresów email

### 3. States (NGXS)
- **suppliers.action.ts** - Akcje: LoadSuppliers, ChangePage, ApplyFilter, AddSupplier, UpdateSupplier, DeleteSupplier
- **suppliers.state.ts** - State zarządzający danymi dostawców
- **suppliers.state.model.ts** - Model state

### 4. Services
- **suppliers.service.ts** - Serwis komunikacji z API (`/procurement/suppliers`)

### 5. Pages
- **pages/suppliers/suppliers.component.ts/html/scss** - Główny komponent strony

### 6. Components

#### suppliers-list
- Lista dostawców w tabeli z kolumnami:
  - **Nazwa dostawcy** (+ data modyfikacji)
  - **Numer telefonu**
  - **Adresy email** (liczba + lista)
  - **Akcje** (edycja, usuwanie)

#### suppliers-filters
- Pole wyszukiwania po nazwie dostawcy
- Debounce 300ms

#### suppliers-fixed-buttons
- Przycisk "Dodaj dostawcę" (fixed bottom-right)
- Przycisk "Odśwież" (fixed bottom-right)

#### supplier-form-dialog
- Formularz reaktywny do dodawania/edycji dostawcy
- Pola:
  - Nazwa dostawcy (wymagane)
  - Numer telefonu (opcjonalne)
  - Lista adresów email (minimum 1, walidacja email)
- Możliwość dodawania/usuwania adresów email
- Tryby: dodawanie i edycja

## Funkcjonalności

### ✅ Listowanie dostawców
- Paginacja
- Wyświetlanie nazwy, telefonu, liczby emaili
- Data ostatniej modyfikacji

### ✅ Wyszukiwanie
- Filtrowanie po nazwie dostawcy
- Debounce dla lepszej wydajności

### ✅ Dodawanie dostawcy
- Formularz reaktywny z walidacją
- Co najmniej jeden adres email (wymagany)
- Walidacja formatu email
- Komunikaty toast o sukcesie/błędzie

### ✅ Edycja dostawcy
- Ten sam formularz w trybie edycji
- Wczytywanie istniejących danych
- Możliwość modyfikacji wszystkich pól

### ✅ Usuwanie dostawcy
- Dialog potwierdzający
- Komunikaty toast o sukcesie/błędzie

## Styl wizualny

Moduł wykorzystuje:
- **Tailwind CSS** dla układu i stylizacji
- **Angular Material** dla komponentów UI
- **Gradient backgrounds** i cienie dla kart
- **Spójne kolory** z modułem formats (primary blue: #3f51b5)
- **Animacje hover** na kartach i przyciskach
- **Custom scrollbar** dla listy elementów
- **Numbered badges** dla numeracji elementów

## Routing

Moduł został zintegrowany z routingiem aplikacji:
- **Ścieżka**: `furniture-formats/suppliers`
- **Lazy loading** komponentu
- **Providers**: SuppliersState, SuppliersService
- **Auth guard**: Admin role required

## API Endpoints

```typescript
GET    /graphql - suppliers query (z paginacją)
POST   /procurement/suppliers - utworzenie nowego dostawcy
PUT    /procurement/suppliers/{id} - aktualizacja dostawcy
DELETE /procurement/suppliers/{id} - usunięcie dostawcy
```

## Status

✅ **Build zakończony sukcesem**
✅ **Wszystkie komponenty utworzone**
✅ **State i akcje zaimplementowane**
✅ **Formularz reaktywny z walidacją**
✅ **Integracja z routing**
✅ **Spójny styl z modułem formats**

## Ostrzeżenia budowania

⚠️ `supplier-form-dialog.component.scss` przekroczył budget (3.12 kB / 2.00 kB)
- To nie jest błąd krytyczny, tylko ostrzeżenie o rozmiarze stylów

## Następne kroki

1. Przetestować aplikację w przeglądarce
2. Zweryfikować działanie z prawdziwym API
3. W razie potrzeby dostosować endpoint API
4. Ewentualnie zoptymalizować style SCSS jeśli rozmiar jest problemem

