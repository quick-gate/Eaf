import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class ApiService {
  baseUrl = 'https://localhost:44392/api/eaf/';
  constructor(private httpClient: HttpClient) {
  }

  async callApi<TOutput>(url: string, params: any): Promise<TOutput> {
    return await this.httpClient.post<TOutput>(this.baseUrl + url, params).toPromise();
  }
}
