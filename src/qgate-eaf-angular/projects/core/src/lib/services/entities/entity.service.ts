import { Injectable } from '@angular/core';
import { ApiService } from '../apis/api.service';
import { IPromise } from 'q';
import { EntityList } from '../../dtos/QGate/Eaf/Domain/Components/Entities/EntityList.model';
import { GetEntityListParams } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityListParams.model';
import { GetEntityListResult } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityListResult.model';
import { GetEntityDetailParams } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityDetailParams.model';
import { GetEntityDetailResult } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityDetailResult.model';
import { EntityDetail } from '../../dtos/QGate/Eaf/Domain/Components/Entities/EntityDetail.model';
import { SaveEntityParams } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/SaveEntityParams.model';
import { SaveEntityResult } from '../../dtos/QGate/Eaf/Domain/Entities/Models/Params/SaveEntityResult.model';

@Injectable({
  providedIn: 'root'
})
export class EntityService {

  constructor(private apiService: ApiService) {
  }

  async GetEntityList(params: GetEntityListParams): Promise<EntityList> {

    const result = await this.apiService.callApi<GetEntityListResult>('entities/get-list', params);

    if (!result) {
      return null;
    }

    return result.EntityList;
  }

  async GetEntityDetail(params: GetEntityDetailParams): Promise<EntityDetail> {

    const result = await this.apiService.callApi<GetEntityDetailResult>('entities/get-detail', params);

    if (!result) {
      return null;
    }

    return result.EntityDetail;
  }

  async SaveEntity(params: SaveEntityParams): Promise<any> {

    return await this.apiService.callApi<SaveEntityResult>('entities', params);

  }
}
