export interface CreateShipmentCommand {
  sender: Sender;
  receiver: Receiver;
  parcels: Parcel[];
  insurance: Insurance | null;
  cod: Cod;
  reference: string;
  comments: string;
  external_customer_id: string;
  externalOrderId: string;
}

export interface Address {
  street: string;
  building_number: string;
  city: string;
  post_code: string;
  country_code: string;
}

export interface Sender {
  name: string;
  company_name: string;
  first_name: string;
  last_name: string;
  email: string;
  phone: string;
  address: Address;
}

export interface Receiver {
  name: string;
  company_name: string;
  first_name: string;
  last_name: string;
  email: string;
  phone: string;
  address: Address;
}

export interface Dimensions {
  length: string;
  width: string;
  height: string;
  unit: string;
}

export interface Weight {
  amount: string;
  unit: string;
}

export interface Parcel {
  id: string;
  dimensions: Dimensions;
  weight: Weight;
  is_non_standard: boolean;
}

export interface Insurance {
  amount: number;
  currency: string;
}

export interface Cod {
  amount: number;
  currency: string;
}
