export class RestQueryResponseWithoutPaginationVo<T> {
  public result!: T;
}

export class RestQueryResponse<T> {
  public result!: T;
  public totalCount!: number;
}
