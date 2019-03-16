import { Component, OnInit } from '@angular/core';
import { MetadataService } from '../../../services/metadatas/metadata.service';
import { EntityMetadata } from '../../../dtos/QGate/Eaf/Domain/Metadatas/Models/EntityMetadata.model';


@Component({
  selector: 'eaf-entity-metadata-list',
  templateUrl: './entity-metadata-list.component.html',
  styleUrls: ['./entity-metadata-list.component.css']
})
export class EntityMetadataListComponent implements OnInit {

  model: EntityMetadata[];
  constructor(private metadataService: MetadataService) { }

  async ngOnInit() {
    this.model = await this.metadataService.GetEntityMetadataList();
  }

}
