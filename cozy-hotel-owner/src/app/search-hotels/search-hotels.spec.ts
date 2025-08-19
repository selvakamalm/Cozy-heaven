import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchHotels } from './search-hotels';

describe('SearchHotels', () => {
  let component: SearchHotels;
  let fixture: ComponentFixture<SearchHotels>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchHotels]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchHotels);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
