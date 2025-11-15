import { gql } from 'apollo-angular';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type MakeEmpty<T extends { [key: string]: unknown }, K extends keyof T> = { [_ in K]?: never };
export type Incremental<T> = T | { [P in keyof T]?: P extends ' $fragmentName' | '__typename' ? T[P] : never };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: { input: string; output: string; }
  String: { input: string; output: string; }
  Boolean: { input: boolean; output: boolean; }
  Int: { input: number; output: number; }
  Float: { input: number; output: number; }
  /** The `DateTime` scalar represents an ISO-8601 compliant date time type. */
  DateTime: { input: any; output: any; }
  /** The `Long` scalar type represents non-fractional signed whole 64-bit numeric values. Long can represent values between -(2^63) and 2^63 - 1. */
  Long: { input: any; output: any; }
  UUID: { input: any; output: any; }
};

/** Defines when a policy shall be executed. */
export enum ApplyPolicy {
  /** After the resolver was executed. */
  AfterResolver = 'AFTER_RESOLVER',
  /** Before the resolver was executed. */
  BeforeResolver = 'BEFORE_RESOLVER',
  /** The policy is applied in the validation step before the execution. */
  Validation = 'VALIDATION'
}

export type BooleanOperationFilterInput = {
  eq?: InputMaybe<Scalars['Boolean']['input']>;
  neq?: InputMaybe<Scalars['Boolean']['input']>;
};

export type DateTimeOperationFilterInput = {
  eq?: InputMaybe<Scalars['DateTime']['input']>;
  gt?: InputMaybe<Scalars['DateTime']['input']>;
  gte?: InputMaybe<Scalars['DateTime']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['DateTime']['input']>>>;
  lt?: InputMaybe<Scalars['DateTime']['input']>;
  lte?: InputMaybe<Scalars['DateTime']['input']>;
  neq?: InputMaybe<Scalars['DateTime']['input']>;
  ngt?: InputMaybe<Scalars['DateTime']['input']>;
  ngte?: InputMaybe<Scalars['DateTime']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['DateTime']['input']>>>;
  nlt?: InputMaybe<Scalars['DateTime']['input']>;
  nlte?: InputMaybe<Scalars['DateTime']['input']>;
};

export type DimensionsViewModel = {
  __typename?: 'DimensionsViewModel';
  lengthEdgeBandingType: EdgeBandingTypeViewModel;
  lengthMm: Scalars['Int']['output'];
  thicknessMm: Scalars['Int']['output'];
  widthEdgeBandingType: EdgeBandingTypeViewModel;
  widthMm: Scalars['Int']['output'];
};

export type DimensionsViewModelFilterInput = {
  and?: InputMaybe<Array<DimensionsViewModelFilterInput>>;
  lengthEdgeBandingType?: InputMaybe<EdgeBandingTypeViewModelOperationFilterInput>;
  lengthMm?: InputMaybe<IntOperationFilterInput>;
  or?: InputMaybe<Array<DimensionsViewModelFilterInput>>;
  thicknessMm?: InputMaybe<IntOperationFilterInput>;
  widthEdgeBandingType?: InputMaybe<EdgeBandingTypeViewModelOperationFilterInput>;
  widthMm?: InputMaybe<IntOperationFilterInput>;
};

export type DpdShipmentViewModel = {
  __typename?: 'DpdShipmentViewModel';
  createdAt: Scalars['DateTime']['output'];
  externalOrderNumber: Scalars['String']['output'];
  shipmentNumber: Scalars['String']['output'];
  tenantId: Scalars['UUID']['output'];
};

export type DpdShipmentViewModelFilterInput = {
  and?: InputMaybe<Array<DpdShipmentViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  externalOrderNumber?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<DpdShipmentViewModelFilterInput>>;
  shipmentNumber?: InputMaybe<StringOperationFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
};

export enum EdgeBandingTypeViewModel {
  None = 'NONE',
  One = 'ONE',
  Two = 'TWO'
}

export type EdgeBandingTypeViewModelOperationFilterInput = {
  eq?: InputMaybe<EdgeBandingTypeViewModel>;
  in?: InputMaybe<Array<EdgeBandingTypeViewModel>>;
  neq?: InputMaybe<EdgeBandingTypeViewModel>;
  nin?: InputMaybe<Array<EdgeBandingTypeViewModel>>;
};

export type FloatOperationFilterInput = {
  eq?: InputMaybe<Scalars['Float']['input']>;
  gt?: InputMaybe<Scalars['Float']['input']>;
  gte?: InputMaybe<Scalars['Float']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['Float']['input']>>>;
  lt?: InputMaybe<Scalars['Float']['input']>;
  lte?: InputMaybe<Scalars['Float']['input']>;
  neq?: InputMaybe<Scalars['Float']['input']>;
  ngt?: InputMaybe<Scalars['Float']['input']>;
  ngte?: InputMaybe<Scalars['Float']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['Float']['input']>>>;
  nlt?: InputMaybe<Scalars['Float']['input']>;
  nlte?: InputMaybe<Scalars['Float']['input']>;
};

export type FurnitureModelViewModel = {
  __typename?: 'FurnitureModelViewModel';
  createdAt: Scalars['DateTime']['output'];
  createdBy: Scalars['UUID']['output'];
  deletedAt?: Maybe<Scalars['DateTime']['output']>;
  furnitureCode: Scalars['String']['output'];
  isDeleted: Scalars['Boolean']['output'];
  modifiedAt: Scalars['DateTime']['output'];
  modifiedBy: Scalars['UUID']['output'];
  partDefinitions: Array<PartDefinitionViewModel>;
  tenantId: Scalars['UUID']['output'];
  version: Scalars['Int']['output'];
};

export type FurnitureModelViewModelFilterInput = {
  and?: InputMaybe<Array<FurnitureModelViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  createdBy?: InputMaybe<UuidOperationFilterInput>;
  deletedAt?: InputMaybe<DateTimeOperationFilterInput>;
  furnitureCode?: InputMaybe<StringOperationFilterInput>;
  isDeleted?: InputMaybe<BooleanOperationFilterInput>;
  modifiedAt?: InputMaybe<DateTimeOperationFilterInput>;
  modifiedBy?: InputMaybe<UuidOperationFilterInput>;
  or?: InputMaybe<Array<FurnitureModelViewModelFilterInput>>;
  partDefinitions?: InputMaybe<ListFilterInputTypeOfPartDefinitionViewModelFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
  version?: InputMaybe<IntOperationFilterInput>;
};

export type FurnitureModelViewModelSortInput = {
  createdAt?: InputMaybe<SortEnumType>;
  createdBy?: InputMaybe<SortEnumType>;
  deletedAt?: InputMaybe<SortEnumType>;
  furnitureCode?: InputMaybe<SortEnumType>;
  isDeleted?: InputMaybe<SortEnumType>;
  modifiedAt?: InputMaybe<SortEnumType>;
  modifiedBy?: InputMaybe<SortEnumType>;
  tenantId?: InputMaybe<SortEnumType>;
  version?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type FurnitureModelsConnection = {
  __typename?: 'FurnitureModelsConnection';
  /** A list of edges. */
  edges?: Maybe<Array<FurnitureModelsEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<FurnitureModelViewModel>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type FurnitureModelsEdge = {
  __typename?: 'FurnitureModelsEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: FurnitureModelViewModel;
};

export type InpostShipmentViewModel = {
  __typename?: 'InpostShipmentViewModel';
  createdAt: Scalars['DateTime']['output'];
  externalOrderNumber: Scalars['String']['output'];
  shipmentNumber: Scalars['String']['output'];
  tenantId: Scalars['UUID']['output'];
};

export type InpostShipmentViewModelFilterInput = {
  and?: InputMaybe<Array<InpostShipmentViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  externalOrderNumber?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<InpostShipmentViewModelFilterInput>>;
  shipmentNumber?: InputMaybe<StringOperationFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
};

export type IntOperationFilterInput = {
  eq?: InputMaybe<Scalars['Int']['input']>;
  gt?: InputMaybe<Scalars['Int']['input']>;
  gte?: InputMaybe<Scalars['Int']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['Int']['input']>>>;
  lt?: InputMaybe<Scalars['Int']['input']>;
  lte?: InputMaybe<Scalars['Int']['input']>;
  neq?: InputMaybe<Scalars['Int']['input']>;
  ngt?: InputMaybe<Scalars['Int']['input']>;
  ngte?: InputMaybe<Scalars['Int']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['Int']['input']>>>;
  nlt?: InputMaybe<Scalars['Int']['input']>;
  nlte?: InputMaybe<Scalars['Int']['input']>;
};

export type IntegratorQuery = {
  __typename?: 'IntegratorQuery';
  barcodeScannerLogs: Array<StockLogViewModel>;
  dpdShipments: Array<DpdShipmentViewModel>;
  furnitureModels?: Maybe<FurnitureModelsConnection>;
  inpostShipments: Array<InpostShipmentViewModel>;
  invoices: Array<InvoiceViewModel>;
  partDefinitionOrders?: Maybe<PartDefinitionOrdersConnection>;
  shipments: Array<ShipmentViewModel>;
  stockGroups: Array<StockGroupViewModel>;
  stocks: Array<StockViewModel>;
  suppliers?: Maybe<SuppliersConnection>;
  tagParcelTemplates?: Maybe<TagParcelTemplatesConnection>;
};


export type IntegratorQueryBarcodeScannerLogsArgs = {
  order?: InputMaybe<Array<StockLogViewModelSortInput>>;
  where?: InputMaybe<StockLogViewModelFilterInput>;
};


export type IntegratorQueryDpdShipmentsArgs = {
  where?: InputMaybe<DpdShipmentViewModelFilterInput>;
};


export type IntegratorQueryFurnitureModelsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<FurnitureModelViewModelSortInput>>;
  where?: InputMaybe<FurnitureModelViewModelFilterInput>;
};


export type IntegratorQueryInpostShipmentsArgs = {
  where?: InputMaybe<InpostShipmentViewModelFilterInput>;
};


export type IntegratorQueryInvoicesArgs = {
  order?: InputMaybe<Array<InvoiceViewModelSortInput>>;
  where?: InputMaybe<InvoiceViewModelFilterInput>;
};


export type IntegratorQueryPartDefinitionOrdersArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<PartDefinitionsOrderViewModelSortInput>>;
  where?: InputMaybe<PartDefinitionsOrderViewModelFilterInput>;
};


export type IntegratorQueryShipmentsArgs = {
  where?: InputMaybe<ShipmentViewModelFilterInput>;
};


export type IntegratorQueryStockGroupsArgs = {
  order?: InputMaybe<Array<StockGroupViewModelSortInput>>;
  where?: InputMaybe<StockGroupViewModelFilterInput>;
};


export type IntegratorQueryStocksArgs = {
  order?: InputMaybe<Array<StockViewModelSortInput>>;
  where?: InputMaybe<StockViewModelFilterInput>;
};


export type IntegratorQuerySuppliersArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<SupplierViewModelSortInput>>;
  where?: InputMaybe<SupplierViewModelFilterInput>;
};


export type IntegratorQueryTagParcelTemplatesArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<TagParcelTemplateViewModelSortInput>>;
  where?: InputMaybe<TagParcelTemplateViewModelFilterInput>;
};

export type InvoiceViewModel = {
  __typename?: 'InvoiceViewModel';
  createdAt: Scalars['DateTime']['output'];
  externalOrderNumber: Scalars['String']['output'];
  invoiceId: Scalars['Long']['output'];
  invoiceNumber: Scalars['String']['output'];
  tenantId: Scalars['UUID']['output'];
};

export type InvoiceViewModelFilterInput = {
  and?: InputMaybe<Array<InvoiceViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  externalOrderNumber?: InputMaybe<StringOperationFilterInput>;
  invoiceId?: InputMaybe<LongOperationFilterInput>;
  invoiceNumber?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<InvoiceViewModelFilterInput>>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
};

export type InvoiceViewModelSortInput = {
  createdAt?: InputMaybe<SortEnumType>;
  externalOrderNumber?: InputMaybe<SortEnumType>;
  invoiceId?: InputMaybe<SortEnumType>;
  invoiceNumber?: InputMaybe<SortEnumType>;
  tenantId?: InputMaybe<SortEnumType>;
};

export type ListFilterInputTypeOfOrderFurnitureLineViewModelFilterInput = {
  all?: InputMaybe<OrderFurnitureLineViewModelFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<OrderFurnitureLineViewModelFilterInput>;
  some?: InputMaybe<OrderFurnitureLineViewModelFilterInput>;
};

export type ListFilterInputTypeOfOrderFurniturePartLineViewModelFilterInput = {
  all?: InputMaybe<OrderFurniturePartLineViewModelFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<OrderFurniturePartLineViewModelFilterInput>;
  some?: InputMaybe<OrderFurniturePartLineViewModelFilterInput>;
};

export type ListFilterInputTypeOfPartDefinitionViewModelFilterInput = {
  all?: InputMaybe<PartDefinitionViewModelFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<PartDefinitionViewModelFilterInput>;
  some?: InputMaybe<PartDefinitionViewModelFilterInput>;
};

export type ListFilterInputTypeOfStockLogViewModelFilterInput = {
  all?: InputMaybe<StockLogViewModelFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<StockLogViewModelFilterInput>;
  some?: InputMaybe<StockLogViewModelFilterInput>;
};

export type ListFilterInputTypeOfStockViewModelFilterInput = {
  all?: InputMaybe<StockViewModelFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<StockViewModelFilterInput>;
  some?: InputMaybe<StockViewModelFilterInput>;
};

export type ListFilterInputTypeOfSupplierMailboxViewModelFilterInput = {
  all?: InputMaybe<SupplierMailboxViewModelFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<SupplierMailboxViewModelFilterInput>;
  some?: InputMaybe<SupplierMailboxViewModelFilterInput>;
};

export type ListFilterInputTypeOfTagParcelViewModelFilterInput = {
  all?: InputMaybe<TagParcelViewModelFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<TagParcelViewModelFilterInput>;
  some?: InputMaybe<TagParcelViewModelFilterInput>;
};

export type LongOperationFilterInput = {
  eq?: InputMaybe<Scalars['Long']['input']>;
  gt?: InputMaybe<Scalars['Long']['input']>;
  gte?: InputMaybe<Scalars['Long']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['Long']['input']>>>;
  lt?: InputMaybe<Scalars['Long']['input']>;
  lte?: InputMaybe<Scalars['Long']['input']>;
  neq?: InputMaybe<Scalars['Long']['input']>;
  ngt?: InputMaybe<Scalars['Long']['input']>;
  ngte?: InputMaybe<Scalars['Long']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['Long']['input']>>>;
  nlt?: InputMaybe<Scalars['Long']['input']>;
  nlte?: InputMaybe<Scalars['Long']['input']>;
};

export type OrderFurnitureLineViewModel = {
  __typename?: 'OrderFurnitureLineViewModel';
  furnitureCode: Scalars['String']['output'];
  furnitureVersion: Scalars['Int']['output'];
  id: Scalars['Int']['output'];
  orderId: Scalars['Int']['output'];
  partLines: Array<OrderFurniturePartLineViewModel>;
  quantityOrdered: Scalars['Int']['output'];
  tenantId: Scalars['UUID']['output'];
};

export type OrderFurnitureLineViewModelFilterInput = {
  and?: InputMaybe<Array<OrderFurnitureLineViewModelFilterInput>>;
  furnitureCode?: InputMaybe<StringOperationFilterInput>;
  furnitureVersion?: InputMaybe<IntOperationFilterInput>;
  id?: InputMaybe<IntOperationFilterInput>;
  or?: InputMaybe<Array<OrderFurnitureLineViewModelFilterInput>>;
  orderId?: InputMaybe<IntOperationFilterInput>;
  partLines?: InputMaybe<ListFilterInputTypeOfOrderFurniturePartLineViewModelFilterInput>;
  quantityOrdered?: InputMaybe<IntOperationFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
};

export enum OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel {
  None = 'NONE',
  One = 'ONE',
  Two = 'TWO'
}

export type OrderFurniturePartLineDimensionsEdgeBandingTypeViewModelOperationFilterInput = {
  eq?: InputMaybe<OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel>;
  in?: InputMaybe<Array<OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel>>;
  neq?: InputMaybe<OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel>;
  nin?: InputMaybe<Array<OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel>>;
};

export type OrderFurniturePartLineDimensionsViewModel = {
  __typename?: 'OrderFurniturePartLineDimensionsViewModel';
  lengthEdgeBandingType: OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel;
  lengthMm: Scalars['Int']['output'];
  thicknessMm: Scalars['Int']['output'];
  widthEdgeBandingType: OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel;
  widthMm: Scalars['Int']['output'];
};

export type OrderFurniturePartLineDimensionsViewModelFilterInput = {
  and?: InputMaybe<Array<OrderFurniturePartLineDimensionsViewModelFilterInput>>;
  lengthEdgeBandingType?: InputMaybe<OrderFurniturePartLineDimensionsEdgeBandingTypeViewModelOperationFilterInput>;
  lengthMm?: InputMaybe<IntOperationFilterInput>;
  or?: InputMaybe<Array<OrderFurniturePartLineDimensionsViewModelFilterInput>>;
  thicknessMm?: InputMaybe<IntOperationFilterInput>;
  widthEdgeBandingType?: InputMaybe<OrderFurniturePartLineDimensionsEdgeBandingTypeViewModelOperationFilterInput>;
  widthMm?: InputMaybe<IntOperationFilterInput>;
};

export type OrderFurniturePartLineViewModel = {
  __typename?: 'OrderFurniturePartLineViewModel';
  additionalInfo?: Maybe<Scalars['String']['output']>;
  dimensions: OrderFurniturePartLineDimensionsViewModel;
  id: Scalars['Int']['output'];
  name: Scalars['String']['output'];
  orderFurnitureLineId: Scalars['Int']['output'];
  quantity: Scalars['Int']['output'];
};

export type OrderFurniturePartLineViewModelFilterInput = {
  additionalInfo?: InputMaybe<StringOperationFilterInput>;
  and?: InputMaybe<Array<OrderFurniturePartLineViewModelFilterInput>>;
  dimensions?: InputMaybe<OrderFurniturePartLineDimensionsViewModelFilterInput>;
  id?: InputMaybe<IntOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<OrderFurniturePartLineViewModelFilterInput>>;
  orderFurnitureLineId?: InputMaybe<IntOperationFilterInput>;
  quantity?: InputMaybe<IntOperationFilterInput>;
};

export enum OrderStatusViewModel {
  Registered = 'REGISTERED',
  Sent = 'SENT'
}

export type OrderStatusViewModelOperationFilterInput = {
  eq?: InputMaybe<OrderStatusViewModel>;
  in?: InputMaybe<Array<OrderStatusViewModel>>;
  neq?: InputMaybe<OrderStatusViewModel>;
  nin?: InputMaybe<Array<OrderStatusViewModel>>;
};

/** Information about pagination in a connection. */
export type PageInfo = {
  __typename?: 'PageInfo';
  /** When paginating forwards, the cursor to continue. */
  endCursor?: Maybe<Scalars['String']['output']>;
  /** Indicates whether more edges exist following the set defined by the clients arguments. */
  hasNextPage: Scalars['Boolean']['output'];
  /** Indicates whether more edges exist prior the set defined by the clients arguments. */
  hasPreviousPage: Scalars['Boolean']['output'];
  /** When paginating backwards, the cursor to continue. */
  startCursor?: Maybe<Scalars['String']['output']>;
};

/** A connection to a list of items. */
export type PartDefinitionOrdersConnection = {
  __typename?: 'PartDefinitionOrdersConnection';
  /** A list of edges. */
  edges?: Maybe<Array<PartDefinitionOrdersEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<PartDefinitionsOrderViewModel>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type PartDefinitionOrdersEdge = {
  __typename?: 'PartDefinitionOrdersEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: PartDefinitionsOrderViewModel;
};

export type PartDefinitionViewModel = {
  __typename?: 'PartDefinitionViewModel';
  additionalInfo?: Maybe<Scalars['String']['output']>;
  dimensions: DimensionsViewModel;
  furnitureCode: Scalars['String']['output'];
  id: Scalars['Int']['output'];
  name: Scalars['String']['output'];
  quantity: Scalars['Int']['output'];
  tenantId: Scalars['UUID']['output'];
};

export type PartDefinitionViewModelFilterInput = {
  additionalInfo?: InputMaybe<StringOperationFilterInput>;
  and?: InputMaybe<Array<PartDefinitionViewModelFilterInput>>;
  dimensions?: InputMaybe<DimensionsViewModelFilterInput>;
  furnitureCode?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<IntOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<PartDefinitionViewModelFilterInput>>;
  quantity?: InputMaybe<IntOperationFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
};

export type PartDefinitionsOrderViewModel = {
  __typename?: 'PartDefinitionsOrderViewModel';
  additionalNotes?: Maybe<Scalars['String']['output']>;
  createdAt: Scalars['DateTime']['output'];
  createdBy: Scalars['UUID']['output'];
  furnitureLines: Array<OrderFurnitureLineViewModel>;
  id: Scalars['Int']['output'];
  modifiedAt: Scalars['DateTime']['output'];
  modifiedBy: Scalars['UUID']['output'];
  number: Scalars['String']['output'];
  status: OrderStatusViewModel;
  supplierId: Scalars['Int']['output'];
  tenantId: Scalars['UUID']['output'];
};

export type PartDefinitionsOrderViewModelFilterInput = {
  additionalNotes?: InputMaybe<StringOperationFilterInput>;
  and?: InputMaybe<Array<PartDefinitionsOrderViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  createdBy?: InputMaybe<UuidOperationFilterInput>;
  furnitureLines?: InputMaybe<ListFilterInputTypeOfOrderFurnitureLineViewModelFilterInput>;
  id?: InputMaybe<IntOperationFilterInput>;
  modifiedAt?: InputMaybe<DateTimeOperationFilterInput>;
  modifiedBy?: InputMaybe<UuidOperationFilterInput>;
  number?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<PartDefinitionsOrderViewModelFilterInput>>;
  status?: InputMaybe<OrderStatusViewModelOperationFilterInput>;
  supplierId?: InputMaybe<IntOperationFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
};

export type PartDefinitionsOrderViewModelSortInput = {
  additionalNotes?: InputMaybe<SortEnumType>;
  createdAt?: InputMaybe<SortEnumType>;
  createdBy?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  modifiedAt?: InputMaybe<SortEnumType>;
  modifiedBy?: InputMaybe<SortEnumType>;
  number?: InputMaybe<SortEnumType>;
  status?: InputMaybe<SortEnumType>;
  supplierId?: InputMaybe<SortEnumType>;
  tenantId?: InputMaybe<SortEnumType>;
};

export type ShipmentViewModel = {
  __typename?: 'ShipmentViewModel';
  createdAt: Scalars['DateTime']['output'];
  externalOrderNumber: Scalars['String']['output'];
  shipmentNumber: Scalars['String']['output'];
  shipmentProvider: Scalars['String']['output'];
  tenantId: Scalars['UUID']['output'];
};

export type ShipmentViewModelFilterInput = {
  and?: InputMaybe<Array<ShipmentViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  externalOrderNumber?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ShipmentViewModelFilterInput>>;
  shipmentNumber?: InputMaybe<StringOperationFilterInput>;
  shipmentProvider?: InputMaybe<StringOperationFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
};

export enum SortEnumType {
  Asc = 'ASC',
  Desc = 'DESC'
}

export type StockGroupViewModel = {
  __typename?: 'StockGroupViewModel';
  description: Scalars['String']['output'];
  id: Scalars['Int']['output'];
  name: Scalars['String']['output'];
  stocks: Array<StockViewModel>;
  tenantId: Scalars['UUID']['output'];
};

export type StockGroupViewModelFilterInput = {
  and?: InputMaybe<Array<StockGroupViewModelFilterInput>>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<IntOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<StockGroupViewModelFilterInput>>;
  stocks?: InputMaybe<ListFilterInputTypeOfStockViewModelFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
};

export type StockGroupViewModelSortInput = {
  description?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  tenantId?: InputMaybe<SortEnumType>;
};

export enum StockLogStatus {
  Active = 'ACTIVE',
  Reverted = 'REVERTED'
}

export type StockLogStatusOperationFilterInput = {
  eq?: InputMaybe<StockLogStatus>;
  in?: InputMaybe<Array<StockLogStatus>>;
  neq?: InputMaybe<StockLogStatus>;
  nin?: InputMaybe<Array<StockLogStatus>>;
};

export type StockLogViewModel = {
  __typename?: 'StockLogViewModel';
  changeQuantity: Scalars['Int']['output'];
  createdAt: Scalars['DateTime']['output'];
  createdBy: Scalars['String']['output'];
  id: Scalars['Int']['output'];
  packageCode: Scalars['String']['output'];
  scanId: Scalars['String']['output'];
  status: StockLogStatus;
  tenantId: Scalars['UUID']['output'];
};

export type StockLogViewModelFilterInput = {
  and?: InputMaybe<Array<StockLogViewModelFilterInput>>;
  changeQuantity?: InputMaybe<IntOperationFilterInput>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  createdBy?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<IntOperationFilterInput>;
  or?: InputMaybe<Array<StockLogViewModelFilterInput>>;
  packageCode?: InputMaybe<StringOperationFilterInput>;
  scanId?: InputMaybe<StringOperationFilterInput>;
  status?: InputMaybe<StockLogStatusOperationFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
};

export type StockLogViewModelSortInput = {
  changeQuantity?: InputMaybe<SortEnumType>;
  createdAt?: InputMaybe<SortEnumType>;
  createdBy?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  packageCode?: InputMaybe<SortEnumType>;
  scanId?: InputMaybe<SortEnumType>;
  status?: InputMaybe<SortEnumType>;
  tenantId?: InputMaybe<SortEnumType>;
};

export type StockViewModel = {
  __typename?: 'StockViewModel';
  groupId?: Maybe<Scalars['Int']['output']>;
  logs: Array<StockLogViewModel>;
  packageCode: Scalars['String']['output'];
  quantity: Scalars['Int']['output'];
  tenantId: Scalars['UUID']['output'];
  threshold: Scalars['Int']['output'];
};

export type StockViewModelFilterInput = {
  and?: InputMaybe<Array<StockViewModelFilterInput>>;
  groupId?: InputMaybe<IntOperationFilterInput>;
  logs?: InputMaybe<ListFilterInputTypeOfStockLogViewModelFilterInput>;
  or?: InputMaybe<Array<StockViewModelFilterInput>>;
  packageCode?: InputMaybe<StringOperationFilterInput>;
  quantity?: InputMaybe<IntOperationFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
  threshold?: InputMaybe<IntOperationFilterInput>;
};

export type StockViewModelSortInput = {
  groupId?: InputMaybe<SortEnumType>;
  packageCode?: InputMaybe<SortEnumType>;
  quantity?: InputMaybe<SortEnumType>;
  tenantId?: InputMaybe<SortEnumType>;
  threshold?: InputMaybe<SortEnumType>;
};

export type StringOperationFilterInput = {
  and?: InputMaybe<Array<StringOperationFilterInput>>;
  contains?: InputMaybe<Scalars['String']['input']>;
  endsWith?: InputMaybe<Scalars['String']['input']>;
  eq?: InputMaybe<Scalars['String']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  ncontains?: InputMaybe<Scalars['String']['input']>;
  nendsWith?: InputMaybe<Scalars['String']['input']>;
  neq?: InputMaybe<Scalars['String']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  nstartsWith?: InputMaybe<Scalars['String']['input']>;
  or?: InputMaybe<Array<StringOperationFilterInput>>;
  startsWith?: InputMaybe<Scalars['String']['input']>;
};

export type SupplierMailboxViewModel = {
  __typename?: 'SupplierMailboxViewModel';
  email: Scalars['String']['output'];
  id: Scalars['Int']['output'];
  supplierId: Scalars['Int']['output'];
};

export type SupplierMailboxViewModelFilterInput = {
  and?: InputMaybe<Array<SupplierMailboxViewModelFilterInput>>;
  email?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<IntOperationFilterInput>;
  or?: InputMaybe<Array<SupplierMailboxViewModelFilterInput>>;
  supplierId?: InputMaybe<IntOperationFilterInput>;
};

export type SupplierViewModel = {
  __typename?: 'SupplierViewModel';
  createdAt: Scalars['DateTime']['output'];
  createdBy: Scalars['UUID']['output'];
  id: Scalars['Int']['output'];
  mailboxes: Array<SupplierMailboxViewModel>;
  modifiedAt: Scalars['DateTime']['output'];
  modifiedBy: Scalars['UUID']['output'];
  name: Scalars['String']['output'];
  telephoneNumber: Scalars['String']['output'];
  tenantId: Scalars['UUID']['output'];
};

export type SupplierViewModelFilterInput = {
  and?: InputMaybe<Array<SupplierViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  createdBy?: InputMaybe<UuidOperationFilterInput>;
  id?: InputMaybe<IntOperationFilterInput>;
  mailboxes?: InputMaybe<ListFilterInputTypeOfSupplierMailboxViewModelFilterInput>;
  modifiedAt?: InputMaybe<DateTimeOperationFilterInput>;
  modifiedBy?: InputMaybe<UuidOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<SupplierViewModelFilterInput>>;
  telephoneNumber?: InputMaybe<StringOperationFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
};

export type SupplierViewModelSortInput = {
  createdAt?: InputMaybe<SortEnumType>;
  createdBy?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  modifiedAt?: InputMaybe<SortEnumType>;
  modifiedBy?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  telephoneNumber?: InputMaybe<SortEnumType>;
  tenantId?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type SuppliersConnection = {
  __typename?: 'SuppliersConnection';
  /** A list of edges. */
  edges?: Maybe<Array<SuppliersEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<SupplierViewModel>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type SuppliersEdge = {
  __typename?: 'SuppliersEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: SupplierViewModel;
};

export type TagParcelTemplateViewModel = {
  __typename?: 'TagParcelTemplateViewModel';
  parcels: Array<TagParcelViewModel>;
  tag: Scalars['String']['output'];
  tenantId: Scalars['UUID']['output'];
};

export type TagParcelTemplateViewModelFilterInput = {
  and?: InputMaybe<Array<TagParcelTemplateViewModelFilterInput>>;
  or?: InputMaybe<Array<TagParcelTemplateViewModelFilterInput>>;
  parcels?: InputMaybe<ListFilterInputTypeOfTagParcelViewModelFilterInput>;
  tag?: InputMaybe<StringOperationFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
};

export type TagParcelTemplateViewModelSortInput = {
  tag?: InputMaybe<SortEnumType>;
  tenantId?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type TagParcelTemplatesConnection = {
  __typename?: 'TagParcelTemplatesConnection';
  /** A list of edges. */
  edges?: Maybe<Array<TagParcelTemplatesEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<TagParcelTemplateViewModel>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type TagParcelTemplatesEdge = {
  __typename?: 'TagParcelTemplatesEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: TagParcelTemplateViewModel;
};

export type TagParcelViewModel = {
  __typename?: 'TagParcelViewModel';
  height: Scalars['Float']['output'];
  id: Scalars['Int']['output'];
  length: Scalars['Float']['output'];
  tag: Scalars['String']['output'];
  tenantId: Scalars['UUID']['output'];
  weight: Scalars['Float']['output'];
  width: Scalars['Float']['output'];
};

export type TagParcelViewModelFilterInput = {
  and?: InputMaybe<Array<TagParcelViewModelFilterInput>>;
  height?: InputMaybe<FloatOperationFilterInput>;
  id?: InputMaybe<IntOperationFilterInput>;
  length?: InputMaybe<FloatOperationFilterInput>;
  or?: InputMaybe<Array<TagParcelViewModelFilterInput>>;
  tag?: InputMaybe<StringOperationFilterInput>;
  tenantId?: InputMaybe<UuidOperationFilterInput>;
  weight?: InputMaybe<FloatOperationFilterInput>;
  width?: InputMaybe<FloatOperationFilterInput>;
};

export type UuidOperationFilterInput = {
  eq?: InputMaybe<Scalars['UUID']['input']>;
  gt?: InputMaybe<Scalars['UUID']['input']>;
  gte?: InputMaybe<Scalars['UUID']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['UUID']['input']>>>;
  lt?: InputMaybe<Scalars['UUID']['input']>;
  lte?: InputMaybe<Scalars['UUID']['input']>;
  neq?: InputMaybe<Scalars['UUID']['input']>;
  ngt?: InputMaybe<Scalars['UUID']['input']>;
  ngte?: InputMaybe<Scalars['UUID']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['UUID']['input']>>>;
  nlt?: InputMaybe<Scalars['UUID']['input']>;
  nlte?: InputMaybe<Scalars['UUID']['input']>;
};
