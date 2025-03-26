export interface ClienteModel {
  id: number;
  name: string;
  documentNumber: string;
  email: string;
  birthDate: string;
  phone: string;
  isCompany: boolean;
  stateRegistration?: string;
  isExempt?: boolean;
  address: AddressModel;
}

export interface AddressModel {
  zipCode: string;
  street: string;
  number: string;
  neighborhood: string;
  city: string;
  state: string;
}
