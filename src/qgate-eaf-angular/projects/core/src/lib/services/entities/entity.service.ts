import { Injectable } from '@angular/core';
import { ApiService } from '../apis/api.service';

@Injectable({
  providedIn: 'root'
})
export class EntityService {

  constructor(private apiService: ApiService) {
  }

  // async GetEntityList<Entity>
}
