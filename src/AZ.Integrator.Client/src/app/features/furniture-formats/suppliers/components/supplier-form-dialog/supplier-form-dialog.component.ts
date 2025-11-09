import { Component, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Store } from '@ngxs/store';
import { MaterialModule } from '../../../../../shared/modules/material.module';
import { SupplierFormGroup } from '../../models/supplier-form-group.model';
import { SupplierMailboxFormGroupType } from '../../models/supplier-mailbox-form-group.model';
import { SaveSupplierCommand, SupplierMailboxDto } from '../../models/commands/save-supplier.command';
import { AddSupplier, UpdateSupplier } from '../../states/suppliers.action';
import { SupplierViewModel } from '../../../../../shared/graphql/graphql-integrator.schema';

@Component({
  selector: 'app-supplier-form-dialog',
  templateUrl: './supplier-form-dialog.component.html',
  styleUrls: ['./supplier-form-dialog.component.scss'],
  imports: [MaterialModule, FormsModule, ReactiveFormsModule],
  standalone: true,
})
export class SupplierFormDialogComponent implements OnInit {
  private data: SupplierViewModel | null = inject(MAT_DIALOG_DATA);
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<SupplierFormDialogComponent>);
  private store = inject(Store);

  form!: FormGroup<SupplierFormGroup>;
  editMode: boolean = false;

  get mailboxes(): FormArray<SupplierMailboxFormGroupType> {
    return this.form.controls.mailboxes;
  }

  ngOnInit(): void {
    this.editMode = !!this.data?.id;

    this.form = this.fb.group<SupplierFormGroup>({
      name: new FormControl<string>(this.data?.name || '', [Validators.required, Validators.minLength(1)]),
      telephoneNumber: new FormControl<string>(this.data?.telephoneNumber || '', []),
      mailboxes: this.fb.array<SupplierMailboxFormGroupType>([], [Validators.required, Validators.minLength(1)]),
    });

    if (this.data?.mailboxes && this.data.mailboxes.length > 0) {
      this.data.mailboxes.forEach((mailbox: { email: string; id: number }) => {
        this.addMailbox(mailbox.email, mailbox.id);
      });
    } else {
      this.addMailbox();
    }

    this.form.markAllAsTouched();
  }

  addMailbox(email = '', id: number | null = null): void {
    const mailboxGroup = this.fb.group({
      id: new FormControl<number | null>(id),
      email: new FormControl<string>(email, [Validators.required, Validators.email]),
    });

    this.mailboxes.push(mailboxGroup);
  }

  removeMailbox(index: number): void {
    this.mailboxes.removeAt(index);
  }

  onSubmit(): void {
    if (!this.form.valid) {
      return;
    }

    interface MailboxFormValue {
      id?: number | null;
      email?: string | null;
    }

    const mailboxes: SupplierMailboxDto[] = this.form.value.mailboxes!.map((mailbox: MailboxFormValue) => ({
      id: mailbox.id || null,
      email: mailbox.email!,
    }));

    const command: SaveSupplierCommand = {
      id: this.editMode ? this.data!.id : null,
      name: this.form.value.name!,
      telephoneNumber: this.form.value.telephoneNumber || '',
      mailboxes,
    };

    const action = this.editMode ? new UpdateSupplier(command) : new AddSupplier(command);

    this.store.dispatch(action).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: () => {
        // Error handled by state
      },
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
