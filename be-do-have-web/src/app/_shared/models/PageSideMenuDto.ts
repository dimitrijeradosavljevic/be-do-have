export interface PageSideMenuDto {
  id: number;
  title: string;
  open: boolean;
  iconName: string;
  iconColor: string;

  descedants: PageSideMenuDto[];
}
