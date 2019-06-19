import { TestBed } from '@angular/core/testing';

import { EntityUiService } from './entity-ui.service';

describe('EntityUiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EntityUiService = TestBed.get(EntityUiService);
    expect(service).toBeTruthy();
  });
});
