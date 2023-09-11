export interface ISignUp {
  id: string;
  firstName: string;
  secondName: string;
  lastName?: string;
  email: string;
  password: string;
  photo?: File;
}
