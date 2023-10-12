export interface AllegroOrderDetailsModel {
  id: string;
  messageToSeller: string;
  buyer: BuyerDetails;
  payment: PaymentDetails;
  status: string;
  fulfillment: FulfillmentDetails;
  delivery: DeliveryDetails;
  // invoice: InvoiceDetails;
  lineItems: LineItemDetails[];
  surcharges: SurchargeDetails[];
  discounts: DiscountDetails[];
  note: NoteDetails;
  marketplace: MarketplaceDetails;
  summary: SummaryDetails;
  updatedAt: Date;
  revision: string;
}

export interface BuyerDetails {
  id: string;
  email: string;
  login: string;
  firstName: string;
  lastName: string;
  companyName: any;
  guest: boolean;
  personalIdentity: string;
  phoneNumber: any;
  preferences: PreferencesDetails;
  address: AddressDetails;
}

export interface PreferencesDetails {
  language: string;
}

export interface AddressDetails {
  street: string;
  city: string;
  postCode: string;
  countryCode: string;
}

export interface PaymentDetails {
  id: string;
  type: 'CASH_ON_DELIVERY' | 'ONLINE' | string;
  provider: string;
  finishedAt: Date;
  paidAmount: AmountDetails;
  reconciliation: AmountDetails;
}

export interface AmountDetails {
  amount: number;
  currency: string;
}

export interface FulfillmentDetails {
  status: string;
  shipmentSummary: ShipmentSummaryDetails;
}

export interface ShipmentSummaryDetails {
  lineItemsSent: string;
}

export interface DeliveryDetails {
  address: AddressDetails;
  method: MethodDetails;
  pickupPoint: PickupPointDetails;
  cost: AmountDetails;
  time: TimeDetails;
  smart: boolean;
  calculatedNumberOfPackages: number;
}

export interface MethodDetails {
  id: string;
  name: string;
}

export interface PickupPointDetails {
  id: string;
  name: string;
  description: string;
  address: AddressDetails;
}

export interface TimeDetails {
  from: Date;
  to: Date;
  guaranteed: GuaranteedDetails;
  dispatch: DispatchDetails;
}

export interface GuaranteedDetails {
  from: Date;
  to: Date;
}

export interface DispatchDetails {
  from: Date;
  to: Date;
}

// export interface InvoiceDetails {
//     required: boolean;
//     address: AddressDetails;
//     dueDate: Date;
// }

export interface LineItemDetails {
  id: string;
  offer: OfferDetails;
  quantity: number;
  originalPrice: AmountDetails;
  price: AmountDetails;
  reconciliation: ReconciliationDetails;
  selectedAdditionalServices: AdditionalServiceDetails[];
  boughtAt: Date;
}

export interface OfferDetails {
  id: string;
  name: string;
  external: ExternalDetails;
}

export interface ExternalDetails {
  id: string;
}

export interface ReconciliationDetails {
  value: AmountDetails;
  type: string;
  quantity: number;
}

export interface AdditionalServiceDetails {
  definitionId: string;
  name: string;
  price: AmountDetails;
  quantity: number;
}

export interface SurchargeDetails {
  id: string;
  type: string;
  provider: string;
  finishedAt: Date;
  paidAmount: AmountDetails;
  reconciliation: AmountDetails;
}

export interface DiscountDetails {
  type: string;
}

export interface NoteDetails {
  text: string;
}

export interface MarketplaceDetails {
  id: string;
}

export interface SummaryDetails {
  totalToPay: AmountDetails;
}
