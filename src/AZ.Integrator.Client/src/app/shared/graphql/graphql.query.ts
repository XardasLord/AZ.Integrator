import { PageEvent } from '@angular/material/paginator';
import { GraphQLHelper } from './graphql.helper';
import { InputMaybe } from './graphql-integrator.schema';

export class GraphQLQueryVo {
  public currentPage: PageEvent = GraphQLHelper.getInitialPageEvent();
  public currentOrder?: GraphQLOrderVo<any>;
  public searchText?: string = '';
  public filters: Array<GraphQLFilterVo> = new Array<GraphQLFilterVo>();

  constructor(init?: Partial<GraphQLQueryVo>) {
    Object.assign(this, { ...init });
  }
}

export class GraphQLOrderVo<T> {
  public orders!: InputMaybe<Array<T>>;

  constructor(init?: Partial<GraphQLOrderVo<T>>) {
    Object.assign(this, { ...init });
  }
}

export class GraphQLFilterVo {
  public fieldName!: string;
  public fieldValue!: string | number | number[] | boolean | Date;
  public queryType!: GraphQLQueryType;
  public displayFilterLabel?: string;
  public formControlNames?: string[];
  public isHiddenFilter = false;
  public translatedValue?: string;
  public hideFilterValue?: boolean = false;

  constructor(init?: Partial<GraphQLFilterVo>) {
    Object.assign(this, { ...init });
  }
}

export enum GraphQLQueryType {
  StringEqualType = 1,
  IntType = 2,
  BooleanType = 3,
  EnumType = 4,
  DateTypeFromType = 5,
  DateTypeToType = 6,
  DateEqualType = 7,
  StringLikeType = 8,
  IntGreaterEqualThanType = 9,
  IntLessEqualThanType = 10,
  None = 11,
}
