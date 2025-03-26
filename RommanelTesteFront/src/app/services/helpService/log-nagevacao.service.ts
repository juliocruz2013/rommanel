import { Injectable } from '@angular/core';
import { GlobalConstants } from 'src/app/common/global-constants';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class LogNavegacaoService {
  readonly apiURL = GlobalConstants.apiURL;
  readonly headersGet = GlobalConstants.headersGet;
  formData = new FormData();
  ActivatedRouteSnapshot: any;
  RouterStateSnapshot: any;

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {
      this.ActivatedRouteSnapshot = next;
      this.RouterStateSnapshot = state;
  }
}
