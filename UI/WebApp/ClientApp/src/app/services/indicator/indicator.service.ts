import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams, HttpHeaders, HttpResponse } from '@angular/common/http';


export interface IndicatorDto {
    id: string;
    title: string;
    value?: number;
    minValue: number;
    maxValue: number;
    dateValue?: string;
}


@Injectable({
    providedIn: 'root'
})
export class IndicatorService {
  public indicators: IndicatorDto[] = null;
  public load = false;
  public silentLoad = false;

    //private url = 'https://powertowers.azurewebsites.net/api/indicator';
    private url = 'http://localhost:5000/api/indicator';
    constructor(
        private http: HttpClient
    ) {}

  getById(id: string): Observable<IndicatorDto> {
        let params = new HttpParams();


        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json; charset=utf-8',
                Accept: 'application/json'
            }),
            observe: 'response' as 'response',
            params,
            withCredentials: true
        };

    return (this.http.get<IndicatorDto>(this.url + id, 
      httpOptions
    )) as any;
    }

  getAll(): Observable<IndicatorDto[]> {
        let params = new HttpParams();


        return (this.http.get<IndicatorDto[]>(this.url + '/all'));
  }


  update(indicator: IndicatorDto): Observable<HttpResponse<IndicatorDto>> {
    let params = new HttpParams();
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json; charset=utf-8',
        Accept: 'application/json'
      }),
      observe: 'response' as 'response',
      params,
      withCredentials: true
    };

    return this.http.post<IndicatorDto>(
      this.url + '/update',
      indicator,
      httpOptions
    );
  }

  new(indicator: IndicatorDto): Observable<HttpResponse<IndicatorDto>> {
    let params = new HttpParams();
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json; charset=utf-8',
        Accept: 'application/json'
      }),
      observe: 'response' as 'response',
      params,
      withCredentials: true
    };

    return this.http.post<IndicatorDto>(
      this.url + '/new',
      indicator,
      httpOptions
    );
  }

  delete(indicator: IndicatorDto): Observable<HttpResponse<number>> {
    let params = new HttpParams();
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json; charset=utf-8',
        Accept: 'application/json'
      }),
      observe: 'response' as 'response',
      params,
      withCredentials: true
    };

    return this.http.post<number>(
      this.url + '/delete',
      indicator,
      httpOptions
    );
  }

  


}
