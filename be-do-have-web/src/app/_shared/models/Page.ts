import { Tag } from './Tag';
import { User } from './User';

export class Page {
  id?: number;
  userId: number;
  organisationId: number;
  title: string;
  content: string;
  path?: string;
  archived: boolean;
  iconName: string;
  iconColor: string;
  createdAt: string;


  user: User;
  tags: Tag[];
  descendants: Page[];

  directPageId?: number;
}
