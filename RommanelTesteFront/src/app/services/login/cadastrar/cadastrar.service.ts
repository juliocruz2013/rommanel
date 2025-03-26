import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CadastrarModel } from 'src/app/models/login/cadastrar/cadastrar-model';

@Injectable({
  providedIn: 'root',
})
export class CadastrarService {
  private readonly baseUrl = environment.apiURL;

  constructor(private httpClient: HttpClient) { }

  registrarUsuario(usuario: CadastrarModel): Observable<any> {
    return this.httpClient.post<any>(
      this.baseUrl + '/api/v1/users',
      usuario
    );
  }
}
