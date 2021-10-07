import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Store} from '@ngrx/store';
import {State} from '../../../_shared/store';
import {Page} from '../../../_shared/models/Page';
// import {addPage} from '../../store/page.actions';

@Component({
  selector: 'app-page-editor',
  templateUrl: './page-editor.component.html',
  styleUrls: ['./page-editor.component.scss'],
})
export class PageEditorComponent implements OnInit {

  organisationId: number;
  editorForm: FormGroup;
  editorFormActive = false;
  submitted = false;
  loading = false;

  constructor(private formBuilder: FormBuilder,
              private store: Store<State>) { }

  ngOnInit() {
    this.initializeComponent();
  }

  private initializeComponent(): void {
    this.buildEditorForm(null);
  }

  private onSubmit(): void {

    if (this.editorForm.invalid) {
      this.submitted = true;
      return;
    }

    const page = this.editorForm.value as Page;

    this.loading = true;
    if (this.organisationId) {

    }
    else {
      // this.store.dispatch(addPage({ page }));
    }
  }

  private buildEditorForm(page?: Page): void {
    this.editorForm = this.formBuilder.group({
      id: [page ? page.title : null],
      name: [page ? page.title : '', Validators.required]
    });

    this.editorFormActive = true;
  }

}
