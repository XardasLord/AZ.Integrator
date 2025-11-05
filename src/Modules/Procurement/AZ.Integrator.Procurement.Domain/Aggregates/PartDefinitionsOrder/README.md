# PartDefinitionsOrder Aggregate

## Przegląd

Agregat `PartDefinitionsOrder` reprezentuje zamówienie formatek (części mebli) do dostawcy w bounded context **Procurement**. Jest to kluczowy agregat w procesie zamawiania surowców do produkcji mebli.

## Struktura Agregatu

### Główny Agregat: PartDefinitionsOrder

**Odpowiedzialność**: 
- Zarządzanie cyklem życia zamówienia formatek
- Utrzymywanie spójności danych zamówienia
- Przejścia między statusami zamówienia

**Właściwości**:
- `OrderId` (Id) - Strongly-typed identyfikator zamówienia
- `OrderNumber` - Numer zamówienia (Value Object)
- `TenantId` - Identyfikator najemcy (multi-tenancy)
- `SupplierId` - Referencja do agregatu Supplier (tylko ID, bez nawigacji)
- `OrderStatus` - Status zamówienia (Draft → ReadyToSend → Sent)
- `TenantCreationInformation` - Informacje o utworzeniu
- `ModificationInformation` - Informacje o ostatniej modyfikacji
- `FurnitureModelLines` - Kolekcja linii zamówienia dla modeli mebli

**Metody biznesowe**:
- `Create()` - Factory method do tworzenia nowego zamówienia w statusie Draft
- `UpdateFurnitureModelLines()` - Aktualizacja linii zamówienia (tylko w statusie Draft)
- `MarkAsReadyToSend()` - Oznaczenie zamówienia jako gotowego do wysłania
- `MarkAsSent()` - Oznaczenie zamówienia jako wysłanego

### Encja wewnętrzna: FurnitureModelLine

**Odpowiedzialność**:
- Reprezentacja zamówienia dla konkretnego modelu mebla
- Przechowywanie wersji modelu mebla (dla śledzenia zmian w definicjach)
- Zarządzanie ilością zamawianą
- Agregowanie linii definicji części

**Właściwości**:
- `FurnitureModelId` - Referencja do agregatu FurnitureModel z bounded context Catalog (tylko ID)
- `FurnitureModelVersion` - Wersja modelu mebla w momencie tworzenia zamówienia
- `QuantityOrdered` - Ilość zamawiana (Value Object)
- `Lines` - Kolekcja linii definicji części (snapshot)

**Metody**:
- `Create()` - Factory method (internal, tylko dla agregatu)
- `Update()` - Aktualizacja ilości i linii definicji części

### Encja wewnętrzna: PartDefinitionLine

**Odpowiedzialność**:
- Reprezentacja **snapshotu** definicji części w momencie tworzenia zamówienia
- Przechowywanie szczegółów formatki (nazwa, wymiary, ilość, dodatkowe info)

**Właściwości**:
- `PartName` - Nazwa części (Value Object)
- `Dimensions` - Wymiary i obrzeża (Value Object ze szczegółami)
- `Quantity` - Ilość części w jednym meblу (Value Object)
- `AdditionalInfo` - Dodatkowe informacje tekstowe

**Uzasadnienie snapshotu**:
- Definicje formatek w FurnitureModel mogą się zmieniać w czasie
- Zamówienie musi zachować dokładnie to, co zostało zamówione
- Możliwość dodawania niestandardowych pozycji formatek podczas tworzenia zamówienia
- **Brak referencji do PartDefinitionId** - to niezależne dane

## Value Objects

### Dimensions
```csharp
public sealed record Dimensions(
    int LengthMm,              // Długość w mm
    int WidthMm,               // Szerokość w mm
    int ThicknessMm,           // Grubość w mm
    EdgeBandingType LengthEdgeBandingType,  // Obrzeże na długości
    EdgeBandingType WidthEdgeBandingType)   // Obrzeże na szerokości
```

### EdgeBandingType (Enum)
- `None = 0` - Brak obrzeża
- `One = 1` - Jednostronne obrzeże
- `Two = 2` - Dwustronne obrzeże

### OrderStatus (Enum)
- `Draft = 10` - Wersja robocza, możliwość edycji
- `ReadyToSend = 20` - Gotowe do wysłania do dostawcy
- `Sent = 30` - Wysłane (email został wysłany)

### Quantity, PartName, OrderNumber
Standardowe Value Objects z walidacją.

## Data Transfer Objects

### FurnitureModelLineData
DTO dla przekazywania danych o liniach modeli mebli do agregatu.

### PartDefinitionLineData
DTO dla przekazywania danych o liniach definicji części do agregatu.

## Granice Bounded Context

### Referencje do innych agregatów:

1. **Supplier** (ten sam BC: Procurement)
   - Typ: `SupplierId` (strongly-typed ID)
   - Uzasadnienie: Osobny agregat w tym samym BC, tylko referencja przez ID

2. **FurnitureModel** (inny BC: Catalog)
   - Typ: `uint FurnitureModelId` + `int FurnitureModelVersion`
   - Uzasadnienie: Cross-BC reference, tylko ID + wersja dla audytu
   - **Nie ma nawigacji** - zachowanie granic bounded context

## Przejścia stanów zamówienia

```
Draft ──[MarkAsReadyToSend]──> ReadyToSend ──[MarkAsSent]──> Sent
  ↑                                                             ↓
  └───────── Tylko w tym stanie można edytować ─────────────────┘
```

**Reguły biznesowe**:
- Edycja linii zamówienia możliwa tylko w statusie `Draft`
- Przejście do `ReadyToSend` wymaga co najmniej jednej linii mebla
- Przejście do `Sent` tylko z `ReadyToSend`
- Po wysłaniu zamówienie jest niemutowalne

## Wzorce zastosowane

1. **Aggregate Pattern** - PartDefinitionsOrder jako root, kontrola dostępu do encji wewnętrznych
2. **Factory Method Pattern** - metody `Create()` dla tworzenia obiektów
3. **Value Object Pattern** - immutable obiekty wartości z walidacją
4. **Strongly-Typed IDs** - typ-safety dla identyfikatorów
5. **Snapshot Pattern** - przechowywanie snapshotu danych PartDefinition
6. **Encapsulation** - private settery, internal metody dla encji wewnętrznych

## Uwagi dotyczące implementacji

### Dlaczego snapshot zamiast referencji?
```csharp
// ❌ NIE ROBIMY TAK:
public int? PartDefinitionId { get; }  // Referencja do PartDefinition

// ✅ ROBIMY TAK:
public PartName PartName { get; }      // Snapshot danych
public Dimensions Dimensions { get; }
public Quantity Quantity { get; }
```

**Powody**:
1. Definicje formatek mogą się zmieniać w czasie
2. Użytkownik może dodać niestandardowe części przy zamówieniu
3. Audyt - wiemy dokładnie co było zamówione
4. Niezależność od zmian w bounded context Catalog

### Wersjonowanie FurnitureModel
Przechowujemy `FurnitureModelVersion` aby móc:
- Śledzić z jakiej wersji definicji pochodzi zamówienie
- Audytować zmiany w definicjach mebli
- Porównywać historyczne zamówienia

## Dalsze kroki implementacji

### 1. Konfiguracja Entity Framework
Należy stworzyć odpowiednie konfiguracje dla:
- `PartDefinitionsOrderConfiguration`
- `FurnitureModelLineConfiguration`
- `PartDefinitionLineConfiguration`

### 2. Repository
```csharp
public interface IPartDefinitionsOrderRepository : IAggregateRepository<PartDefinitionsOrder>
{
    Task<PartDefinitionsOrder?> GetByNumberAsync(OrderNumber number);
    Task<List<PartDefinitionsOrder>> GetBySupplierId(SupplierId supplierId);
}
```

### 3. Domain Events
Rozważyć dodanie domain events:
- `PartDefinitionsOrderCreated`
- `PartDefinitionsOrderMarkedAsReadyToSend`
- `PartDefinitionsOrderMarkedAsSent` - trigger do wysłania emaila

### 4. Application Layer
- Command Handlers dla Create, Update, MarkAsReadyToSend, MarkAsSent
- Query Handlers dla odczytu zamówień
- DTOs/ViewModels dla API

### 5. Integration Events
- Event do Hangfire dla wysłania emaila po MarkAsSent
- Event handler z subskrypcją do zmian w Catalog BC (opcjonalnie)

## Przykład użycia

```csharp
// Tworzenie nowego zamówienia
var order = PartDefinitionsOrder.Create(
    OrderNumber.Create("ZAM/2025/001"),
    supplierId,
    furnitureModelLinesData,
    currentUser,
    currentDateTime);

await repository.AddAsync(order);

// Aktualizacja linii zamówienia
order.UpdateFurnitureModelLines(
    updatedLinesData,
    currentUser,
    currentDateTime);

// Oznaczenie jako gotowe do wysłania
order.MarkAsReadyToSend(currentUser, currentDateTime);

// Oznaczenie jako wysłane (trigger Hangfire job)
order.MarkAsSent(currentUser, currentDateTime);

await repository.UpdateAsync(order);
```

## Zgodność z architekturą modularnego monolitu

✅ **Bounded Context Isolation** - jasne granice między Procurement a Catalog  
✅ **Aggregate Pattern** - jedna root entity kontrolująca spójność  
✅ **Domain Events** - komunikacja asynchroniczna między modułami  
✅ **Value Objects** - duplikacja typów między BC (np. Dimensions, EdgeBandingType)  
✅ **No Navigation Properties** - tylko IDs między agregatami  
✅ **Encapsulation** - prywatne konstruktory, internal metody  

---

**Autor**: AI Assistant  
**Data**: 2025-10-31  
**Wersja modelu**: 1.0

