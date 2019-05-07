import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ToolbarItemGroupComponent } from './toolbar-item-group.component';

describe('ToolbarItemGroupComponent', () => {
  let component: ToolbarItemGroupComponent;
  let fixture: ComponentFixture<ToolbarItemGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ToolbarItemGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ToolbarItemGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
