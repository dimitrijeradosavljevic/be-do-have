import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { FormControl } from '@angular/forms';

import { Tag } from '../../../_shared/models/Tag';
import { TagService } from '../../services/tag.service';


@Component({
  selector: 'app-tags-picker',
  templateUrl: './tags-picker.component.html',
  styleUrls: ['./tags-picker.component.scss'],
})
export class TagsPickerComponent implements OnInit {

  tags: Tag[];
  filter: FormControl;
  loading = true;
  @Input() withCreate: boolean;
  @Output() tagPicked: EventEmitter<Tag> = new EventEmitter<Tag>();
  tagPaginationConfig = {
    pageIndex: 1,
    pageSize: 10,
    total: undefined
  };

  constructor(private tagService: TagService) { }

  ngOnInit() {
    this.buildFilter();
  }

  buildFilter() {

    this.filter = new FormControl('');
    this.filter.valueChanges
      .pipe(
        debounceTime(350),
        distinctUntilChanged())
      .subscribe(value => {
        this.getTags(value);
      });
    this.getTags('');
  }

  getTags($event: string) {
    this.loading = true;
    this.tagService.getTags($event, this.tagPaginationConfig.pageIndex, this.tagPaginationConfig.pageSize).subscribe(
      result => {
        this.tags = result.items;
        this.tagPaginationConfig.total = result.total
      },
      error => {
        console.log(error);
      },
      () => this.loading = false
    );
  }

  changePage(next: boolean) {
    this.tagPaginationConfig.pageIndex = (next ? this.tagPaginationConfig.pageIndex + 1 : this.tagPaginationConfig.pageIndex - 1);
    this.getTags(this.filter.value);
  }

  pick(tag: Tag) {
    this.tagPicked.emit(tag);
  }

  matchTag() {
    return !this.tags.find(tag => tag.name === this.filter.value);
  }

  createTag() {
    const tag = {
      name: this.filter.value
    } as Tag;

    this.tagService.store(tag).subscribe(
      result => {
        this.tagPicked.emit(result);
      }
    );
  }
}
