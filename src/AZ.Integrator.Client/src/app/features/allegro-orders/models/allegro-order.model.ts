export interface AllegroOrderModel {
  orderId: string;
  date: Date;
  buyer: BuyerModel;
}

export interface BuyerModel {
  id: string;
  email: string;
  login: string;
  guest: boolean;
}
