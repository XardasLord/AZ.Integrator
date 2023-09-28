export interface CreateInpostShipmentCommand {
  sender: Sender;
  receiver: Receiver;
  parcels: Parcel[];
  insurance: Insurance;
  cod: Cod;
  reference: string;
  comments: string;
  externalCustomerId: string;
}

export interface Address {
  street: string;
  buildingNumber: string;
  city: string;
  postCode: string;
  countryCode: string;
}

export interface Sender {
  name: string;
  companyName: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  address: Address;
}

export interface Receiver {
  name: string;
  companyName: string;
  firstName: string;
  lastName: string;
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
  isNonStandard: boolean;
}

export interface Insurance {
  amount: number;
  currency: string;
}

export interface Cod {
  amount: number;
  currency: string;
}
