import { Component, Inject, Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgbActiveModal, NgbDateAdapter, NgbDateParserFormatter, NgbDateStruct, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomAdapter, CustomDateParserFormatter } from './datetime-adapters';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  providers: [
    { provide: NgbDateAdapter, useClass: CustomAdapter },
    { provide: NgbDateParserFormatter, useClass: CustomDateParserFormatter }
  ]
})

export class HomeComponent implements OnInit {
  public gateways: Gateway[];
  public gateway: Gateway;
  private http: HttpClient;
  baseUrl: string;

  gatewayDlgTitle = '';
  gatewayDlgOk = '';
  gatewayForm: FormGroup;
  gatewayEdited: Gateway;
  gatewaySubmited: boolean = false;

  peripheralDlgTitle = '';
  peripheralDlgOk = '';
  peripheralForm: FormGroup;
  peripheralEdited: Peripheral;
  peripheralSubmited: boolean = false;

  ngOnInit(): void {
    this.gatewayForm = this.fb.group({
      serialNumber: ['', [Validators.required, Validators.minLength(3)]],
      name: ['', [Validators.required, Validators.minLength(2)]],
      ipAddress: ['',
        [
          Validators.required,
          Validators.pattern(/^(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$/)
        ]
      ]
    });

    this.peripheralForm = this.fb.group({
      id: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
      vendor: ['', [Validators.required, Validators.minLength(2)]],
      creationDate: ['', [Validators.required]],
      isOnline: ['']
    });
  }

  constructor(http: HttpClient, private fb: FormBuilder, private modalService: NgbModal, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.getAllGateways();
  }

  private getAllGateways() {
    this.http
      .get<Gateway[]>(`${this.baseUrl}Gateway/All`)
      .subscribe(result => {
        this.gateways = result;
      },
        error => {
          if (error.error)
            return alert(error.error);

          alert(`${error.status} - ${error.statusText}`);
        });
  }

  currentGateway(gateway: Gateway) {
    if (this.gateway == gateway) return false;
    this.gateway = gateway;
    return false;
  }

  newGateway(content) {
    this.gatewayDlgTitle = 'New Gateway';
    this.gatewayDlgOk = 'Add';
    this.gatewayEdited = null;
    this.gatewaySubmited = false;
    this.gatewayForm.reset();
    this.gatewayForm.get('serialNumber').enable();
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' });
  }

  editGateway(content: any, gateway: Gateway) {
    this.gatewayDlgTitle = 'Edit Gateway';
    this.gatewayDlgOk = 'Update';
    this.gatewaySubmited = false;
    this.gatewayEdited = gateway;
    this.gatewayForm.reset();
    this.gatewayForm.get('serialNumber').setValue(gateway.serialNumber);
    this.gatewayForm.get('name').setValue(gateway.name);
    this.gatewayForm.get('ipAddress').setValue(gateway.ip);
    this.gatewayForm.get('serialNumber').disable();

    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' });
  }

  validateGateway(modal: NgbActiveModal) {
    this.gatewaySubmited = true;
    this.gatewayForm.markAllAsTouched();
    if (this.gatewayForm.invalid) return;

    if (!this.gatewayEdited)
      this.addNewGateway(modal);
    else
      this.updateGateway(modal);

    return false;
  }

  private addNewGateway(modal: NgbActiveModal) {
    var formData = new FormData();
    formData.append('serialNumber', this.gatewayForm.get('serialNumber').value);
    formData.append('name', this.gatewayForm.get('name').value);
    formData.append('ip', this.gatewayForm.get('ipAddress').value);

    this.http
      .post<Gateway>(`${this.baseUrl}Gateway/Add`, formData)
      .subscribe(result => {
        this.gateways.push(result);

        modal.close();
      },
        error => {
          if (error.error)
            return alert(error.error);

          alert(`${error.status} - ${error.statusText}`);
        });
  }

  private updateGateway(modal: NgbActiveModal) {
    var formData = new FormData();
    formData.append('name', this.gatewayForm.get('name').value);
    formData.append('ip', this.gatewayForm.get('ipAddress').value);

    this.http
      .put<any>(`${this.baseUrl}Gateway/Update/${this.gatewayForm.get('serialNumber').value}`, formData)
      .subscribe(result => {
        this.gatewayEdited.name = this.gatewayForm.get('name').value;
        this.gatewayEdited.ip = this.gatewayForm.get('ipAddress').value;

        modal.close();
      },
        error => {
          if (error.error)
            return alert(error.error);

          alert(`${error.status} - ${error.statusText}`);
        });
  }

  removeGateway(gateway: Gateway) {
    if (!confirm(`Are you sure you want to remove the gateway ${gateway.serialNumber}?`)) return;
    this.http
      .delete(`${this.baseUrl}Gateway/Remove/${gateway.serialNumber}`)
      .subscribe(result => {
        this.gateways.splice(this.gateways.indexOf(gateway), 1);
      },
        error => {
          alert(`Error removing gatway ${gateway.serialNumber}`);
        });
  }

  close(modal: NgbActiveModal) {
    modal.close();
    return false;
  }

  getField(form: FormGroup, field: string) {
    return form.get(field);
  }

  getFieldErrors(form: FormGroup, field: string) {
    const f = form.get(field);
    if (!f.errors) return [];
    return Object.keys(f.errors).map(k => { return { key: k, value: f.errors[k] } });
  }

  isFieldInvalid(form: FormGroup, field: string) {
    const f = form.get(field);
    return f.errors && (f.touched || f.dirty);
  }

  newPeripheral(content) {
    this.peripheralDlgTitle = 'New Peripheral';
    this.peripheralDlgOk = 'Add';
    this.peripheralEdited = null;
    this.peripheralSubmited = false;
    this.peripheralForm.reset();
    this.peripheralForm.get('id').enable();
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' });
  }

  editPeripheral(content: any, peripheral: Peripheral) {
    this.peripheralDlgTitle = 'Edit Peripheral';
    this.peripheralDlgOk = 'Update';
    this.peripheralSubmited = false;
    this.peripheralEdited = peripheral;
    this.peripheralForm.reset();
    this.peripheralForm.get('id').setValue(peripheral.id);
    this.peripheralForm.get('vendor').setValue(peripheral.vendor);
    this.peripheralForm.get('creationDate').setValue(peripheral.creationDate);
    this.peripheralForm.get('isOnline').setValue(peripheral.isOnline);
    this.peripheralForm.get('id').disable();

    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' });
  }

  validatePeripheral(modal: NgbActiveModal) {
    this.peripheralSubmited = true;
    this.peripheralForm.markAllAsTouched();
    console.log(this.peripheralForm);
    if (this.peripheralForm.invalid) return;

    if (!this.peripheralEdited)
      this.addNewPeripheral(modal);
    else
      this.updatePeripheral(modal);

    return false;
  }

  private addNewPeripheral(modal: NgbActiveModal) {
    var formData = new FormData();
    formData.append('id', this.peripheralForm.get('id').value);
    formData.append('vendor', this.peripheralForm.get('vendor').value);
    formData.append('creationDate', this.peripheralForm.get('creationDate').value);
    formData.append('isOnline', this.peripheralForm.get('isOnline').value || 'false');

    this.http
      .post<Peripheral>(`${this.baseUrl}Peripheral/Add/${this.gateway.serialNumber}`, formData)
      .subscribe(result => {
        this.gateway.peripherals.push(result);

        modal.close();
      },
        error => {
          if (error.error)
            return alert(error.error);

          alert(`${error.status} - ${error.statusText}`);
        });
  }

  private updatePeripheral(modal: NgbActiveModal) {
    var formData = new FormData();
    formData.append('vendor', this.peripheralForm.get('vendor').value);
    formData.append('creationDate', this.peripheralForm.get('creationDate').value);
    formData.append('isOnline', this.peripheralForm.get('isOnline').value || 'false');

    this.http
      .put<any>(`${this.baseUrl}Peripheral/Update/${this.peripheralForm.get('id').value}/${this.gateway.serialNumber}`, formData)
      .subscribe(result => {
        this.peripheralEdited.vendor = this.peripheralForm.get('vendor').value;
        this.peripheralEdited.creationDate = this.peripheralForm.get('creationDate').value;
        this.peripheralEdited.isOnline = this.peripheralForm.get('isOnline').value;

        modal.close();
      },
        error => {
          if (error.error)
            return alert(error.error);

          alert(`${error.status} - ${error.statusText}`);
        });
  }

  removePeripheral(peripheral: Peripheral) {
    if (!confirm(`Are you sure you want to remove the peripheral ${peripheral.id}?`)) return;
    this.http
      .delete(`${this.baseUrl}Peripheral/Remove/${peripheral.id}/${this.gateway.serialNumber}`)
      .subscribe(result => {
        this.gateway.peripherals.splice(this.gateway.peripherals.indexOf(peripheral), 1);
      },
        error => {
          alert(`Error removing gatway ${peripheral.id}`);
        });
  }

}

interface Gateway {
  serialNumber: string,
  name: string,
  ip: string,
  peripherals: Peripheral[]
}

interface Peripheral {
  id: number,
  vendor: string,
  creationDate: Date,
  isOnline: boolean
}
