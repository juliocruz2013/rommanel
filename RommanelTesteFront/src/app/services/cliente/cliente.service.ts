import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { ClienteModel } from 'src/app/models/cliente/cliente';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })

export class ClienteService {

  private readonly baseUrl = environment.apiURL;
  constructor(private httpClient: HttpClient) { }

  httpHeaders() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return httpOptions;
  }

  listarClientes(): Observable<ClienteModel[]> {
    return this.httpClient.get<ClienteModel[]>(this.baseUrl + '/api/v1/customers');
  }

  editarCliente(cliente: ClienteModel): Observable<any> {
    return this.httpClient.put<any>(this.baseUrl + '/api/v1/customers', cliente);
  }

  cadastrarCliente(cliente: ClienteModel): Observable<any> {
    return this.httpClient.post<any>(`${this.baseUrl}/api/v1/customers/`, cliente, this.httpHeaders());
  }

  listarClientePorId(id: number): Observable<ClienteModel> {
    return this.httpClient.get<ClienteModel>(`${this.baseUrl}/api/v1/customers/id/${id}`);
  }

  excluirCliente(id: number): Observable<any> {
    return this.httpClient.delete<any>(`${this.baseUrl}/api/v1/customers/id/${id}`);
  }
}
