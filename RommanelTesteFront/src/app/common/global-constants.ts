import { HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';

export class GlobalConstants {
  public static apiURL: string = environment.apiURL;
  public static minutesToken: number = 600;

  public static headersGet = new HttpHeaders({
    'Content-Type': 'application/json',
    Accept: 'application/json',
  });
  public static headersPostNoAuth = new HttpHeaders({
    'Content-Type': 'application/json',
    Accept: 'application/json',
    'No-Auth': 'True',
  });
  public static headersPost = new HttpHeaders({
    'Content-Type': 'application/x-www-form-urlencoded',
  });
}
