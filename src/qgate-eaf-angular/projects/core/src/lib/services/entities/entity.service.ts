import { Injectable } from '@angular/core';
import { ApiService } from '../apis/api.service';
import { EntityList } from '../../dtos/QGate/Eaf/Domain/Components/Entities/EntityList.model';
import { GetEntityListParams } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityListParams.model';
import { GetEntityListResult } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityListResult.model';
import { GetEntityDetailParams } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityDetailParams.model';
import { GetEntityDetailResult } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityDetailResult.model';
import { EntityDetail } from '../../dtos/QGate/Eaf/Domain/Components/Entities/EntityDetail.model';
import { SaveEntityParams } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/SaveEntityParams.model';
import { SaveEntityResult } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/SaveEntityResult.model';
import { DeleteEntityParams } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/DeleteEntityParams.model';
import { DeleteEntityResult } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/DeleteEntityResult.model';

@Injectable({
  providedIn: 'root'
})
export class EntityService {

  constructor(private apiService: ApiService) {
  }

  async getEntityList(params: GetEntityListParams): Promise<EntityList> {

    const result = await this.apiService.callApi<GetEntityListResult>(this.getUri('get-list'), params);

    if (!result) {
      return null;
    }

    return result.EntityList;
  }

  async getEntityDetail(params: GetEntityDetailParams): Promise<EntityDetail> {

    const result = await this.apiService.callApi<GetEntityDetailResult>(this.getUri('get-detail'), params);

    if (!result) {
      return null;
    }

    return result.EntityDetail;
  }

  async saveEntity(params: SaveEntityParams): Promise<SaveEntityResult> {

    return await this.apiService.callApi<SaveEntityResult>(this.getUri(null), params);

  }

  async deleteEntity(params: DeleteEntityParams): Promise<DeleteEntityResult> {

    return await this.apiService.callApi<DeleteEntityResult>(this.getUri('delete'), params);

  }

  private getUri(endpointUrl: string): string {
    const baseUrl = 'entities';
    return `${'entities'}${endpointUrl ? '/' + endpointUrl : ''}`;
  }
}
