export interface PageSearchParameters {

  term?: string;
  titleOnly?: boolean;
  author?: string;
  createdAtStart?: string;
  createdAtEnd?: string;
  updatedAtStart?: string;
  updatedAtEnd?: string;
  pageId?: number;
  organisationId?: number;
  tagIds: number[];
  inPagesSearch?: number[];
}
