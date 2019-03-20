import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UiComponentFactoryComponent } from './ui-component-factory.component';

describe('UiComponentFactoryComponent', () => {
  let component: UiComponentFactoryComponent;
  let fixture: ComponentFixture<UiComponentFactoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UiComponentFactoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UiComponentFactoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
