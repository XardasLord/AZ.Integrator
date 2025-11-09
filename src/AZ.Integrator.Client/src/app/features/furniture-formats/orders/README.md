# Moduł zamówień formatek mebli (Furniture Orders)

## Przegląd
Moduł do zarządzania zamówieniami formatek mebli do dostawców. Umożliwia tworzenie nowych zamówień oraz przeglądanie listy utworzonych zamówień z ich szczegółami.

## Funkcjonalności

### 1. Lista zamówień
- Wyświetlanie wszystkich utworzonych zamówień w formie tabeli
- Kolumny: ID, Numer zamówienia, Data utworzenia, Status, Dostawca
- Paginacja wyników
- Wyszukiwanie po numerze zamówienia
- Rozwijane szczegóły zamówienia z informacjami o:
  - Dostawcy
  - Definicjach mebli w zamówieniu
  - Formatach (partDefinitions) dla każdego mebla

### 2. Tworzenie nowego zamówienia
Wieloetapowy formularz zawierający:

#### Krok 1: Wybór dostawcy
- Lista rozwijana z dostępnymi dostawcami (z modułu Suppliers)

#### Krok 2: Wybór definicji mebli
- Wielokrotny wybór definicji mebli (z modułu Formats)
- Wyświetlanie liczby formatek dla każdej definicji
- Wizualna reprezentacja wybranych mebli

#### Krok 3: Konfiguracja zamówienia
Dla każdej wybranej definicji mebla:
- **Ilość zamówionych mebli** (quantityOrdered)
- **Lista formatek** (edytowalna):
  - Nazwa formatki
  - Wymiary (długość, szerokość, grubość w mm)
  - Oklejka na długości (lengthEdgeBandingType)
  - Oklejka na szerokości (widthEdgeBandingType)
  - Ilość formatki (quantity)
  - Dodatkowe informacje (opcjonalne)
  
#### Edycja formatek:
- Dodawanie nowych formatek
- Usuwanie formatek
- Edycja wszystkich właściwości formatki

#### Krok 4: Wysłanie zamówienia
- Walidacja formularza
- Wysłanie requestu POST do `/api/procurement/orders`

## Struktura katalogów

```
orders/
├── components/
│   ├── orders-list/               # Lista zamówień
│   ├── orders-filters/            # Filtry wyszukiwania
│   ├── orders-fixed-buttons/      # Floating button do dodawania
│   ├── create-order-form/         # Formularz tworzenia zamówienia
│   └── furniture-selector/        # Komponent wyboru definicji mebli
├── graphql-queries/
│   └── get-part-definition-orders.graphql.query.ts
├── models/
│   ├── create-order-request.model.ts      # Model requestu
│   ├── order-form-group.model.ts          # TypeScript interface dla formularza
│   ├── furniture-line-form-group.model.ts # Interface dla linii mebla
│   └── part-line-form-group.model.ts      # Interface dla linii formatki
├── pages/
│   ├── orders/                    # Strona główna z listą
│   └── create-order/              # Strona tworzenia zamówienia
├── services/
│   └── orders.service.ts          # Serwis do komunikacji z API i GraphQL
├── states/
│   ├── orders.action.ts           # Akcje NGXS
│   ├── orders.state.ts            # State NGXS
│   └── orders.state.model.ts      # Model stanu
```

## Modele danych

### CreateOrderRequest
```typescript
{
  supplierId: number;
  furnitureLineRequests: CreateFurnitureLineRequest[];
}
```

### CreateFurnitureLineRequest
```typescript
{
  furnitureCode: string;
  furnitureVersion: number;
  quantityOrdered: number;
  partDefinitionLines: CreateFurniturePartLineRequest[];
}
```

### CreateFurniturePartLineRequest
```typescript
{
  partName: string;
  lengthMm: number;
  widthMm: number;
  thicknessMm: number;
  quantity: number;
  additionalInfo: string;
  lengthEdgeBandingType: EdgeBandingTypeViewModel;
  widthEdgeBandingType: EdgeBandingTypeViewModel;
}
```

## Integracja z innymi modułami

### Suppliers Module
- Pobieranie listy dostawców: `SuppliersState.getSuppliers`
- Wykorzystywane do wyboru dostawcy w formularzu

### Formats Module
- Pobieranie definicji mebli: `FormatsState.getFurnitureDefinitions`
- Wykorzystywane do wyboru mebli i ich formatek

## API Endpoints

### GET (GraphQL)
- Query: `partDefinitionOrders` - pobiera listę zamówień z pełnymi szczegółami

### POST
- Endpoint: `/api/procurement/orders`
- Body: `CreateOrderRequest`
- Tworzy nowe zamówienie formatek

## State Management (NGXS)

### Actions
- `LoadOrders` - ładuje listę zamówień
- `CreateOrder` - tworzy nowe zamówienie
- `ChangePage` - zmienia stronę w paginacji
- `ApplyFilter` - filtruje zamówienia po numerze

### Selectors
- `getOrders` - zwraca listę zamówień
- `getTotalCount` - zwraca całkowitą liczbę zamówień
- `getCurrentPage` - zwraca aktualną stronę
- `getPageSize` - zwraca rozmiar strony
- `getSearchText` - zwraca tekst wyszukiwania

## Technologie i konwencje

- **Reactive Forms** - formularz w pełni reaktywny z walidacją
- **NGXS Store** - zarządzanie stanem aplikacji
- **GraphQL** - pobieranie danych z backendu
- **REST API** - tworzenie zamówień
- **CSS/SCSS** - stylowanie z czystym CSS (bez używania Tailwind `@apply`)
- **Tailwind CSS** - używany bezpośrednio w szablonach HTML
- **Material Design** - komponenty UI z Angular Material
- **Standalone Components** - wszystkie komponenty są standalone

## Walidacja

- Wszystkie pola wymagane są walidowane
- Ilości muszą być większe niż 0
- Nazwa formatki jest wymagana
- Wymiary formatki są wymagane i muszą być > 0
- Typ oklejki jest wymagany

## Uwagi implementacyjne

1. **Brak edycji zamówień** - Lista zamówień jest tylko do odczytu, nie ma możliwości edycji ani usuwania
2. **Kopiowanie definicji** - Formatki z definicji mebla są kopiowane do formularza i można je edytować przed wysłaniem
3. **Walidacja przed wysłaniem** - Formularz blokuje submit jeśli nie jest poprawnie wypełniony
4. **Nawigacja po utworzeniu** - Po pomyślnym utworzeniu zamówienia następuje przekierowanie do listy zamówień
5. **Statusy zamówień**: 
   - REGISTERED - Zarejestrowane
   - SENT - Wysłane

## Routing

- `/furniture-formats/orders` - Lista zamówień
- `/furniture-formats/orders/create` - Tworzenie nowego zamówienia

## Przyszłe rozszerzenia

Potencjalne funkcjonalności do dodania:
- Eksport zamówień do PDF/Excel
- Historia zmian statusu zamówienia
- Powiadomienia e-mail do dostawcy
- Generowanie raportów zamówień
- Filtry zaawansowane (po dacie, statusie, dostawcy)

