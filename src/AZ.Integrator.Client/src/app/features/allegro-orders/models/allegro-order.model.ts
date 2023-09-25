export interface AllegroOrderModel {
  orderId: string;
  date: Date;
  buyer: BuyerModel;
  lineItems: LineItem[];
}

export interface BuyerModel {
  id: string;
  email: string;
  login: string;
  guest: boolean;
}

export interface LineItem {
  id: string;
  itemId: string;
  itemName: string;
  quantity: number;
  originalPrice: Price;
  price: Price;
}

export interface Price {
  amount: number;
  currency: string;
}
