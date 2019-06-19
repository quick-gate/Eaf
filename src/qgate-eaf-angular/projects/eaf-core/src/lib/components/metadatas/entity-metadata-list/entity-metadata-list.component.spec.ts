import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntityMetadataListComponent } from './entity-metadata-list.component';

describe('EntityMetadataListComponent', () => {
  let component: EntityMetadataListComponent;
  let fixture: ComponentFixture<EntityMetadataListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntityMetadataListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityMetadataListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
