export interface IUser {
  id: string;
  firstName: string;
  secondName: string;
  lastName?: string;
  email: string;
  photo?: string;
  accessToken: string;
}
