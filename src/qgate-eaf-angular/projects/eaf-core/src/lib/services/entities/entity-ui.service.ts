import { Injectable } from '@angular/core';
import { RelationAttributeDto } from '../../dtos/QGate/Eaf/Domain/Components/Entities/RelationAttributeDto.model';

@Injectable({
  providedIn: 'root'
})
export class EntityUiService {

  constructor() { }

  fillParentKeysToChild(relationAttributes: RelationAttributeDto[], ownerEntity: any, entity: any) {
      // Owner key must by assigned to related entity
      if (relationAttributes && entity && ownerEntity) {
        for (const relationAttribute of relationAttributes) {
          entity[relationAttribute.LinkedAttribute] = ownerEntity[relationAttribute.Attribute];
        }
      }
  }
}
