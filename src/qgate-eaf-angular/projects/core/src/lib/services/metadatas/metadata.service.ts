import { Injectable } from '@angular/core';
import { ApiService } from '../apis/api.service';
import { EntityMetadata } from '../../dtos/QGate/Eaf/Domain/Metadatas/Models/EntityMetadata.model';

@Injectable({
  providedIn: 'root'
})
export class MetadataService {

  constructor(private apiService: ApiService) {
  }

  async GetEntityMetadataList(): Promise<EntityMetadata[]> {

    return await this.apiService.callApi<EntityMetadata[]>('metadatas/get-entity-list', null);

  }
}
