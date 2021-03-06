import {Component, OnInit} from '@angular/core';
import {ProductService} from '../shared/product.service';
import {FormBuilder, FormControl, Validators} from '@angular/forms';
import * as XLSX from 'xlsx';
import { Products } from '../shared/Models/AllModels';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  countValue = 1;
  addForm;
  fileName = 'Products.xlsx';
  Products: Products[];

  constructor(private service: ProductService, private formBuilder: FormBuilder) {
    this.addForm = this.formBuilder.group({
      name: new FormControl('', Validators.required),
      type: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required),
      count: 1
    });
  }

  ngOnInit(): void {
    this.service.getProducts().subscribe(res => this.Products = res as Products[], error => console.log(error));
  }

  AddProduct(value: any) {
    // tslint:disable-next-line:max-line-length
    this.service.postProducts(value).subscribe(_ => this.service.getProducts().subscribe(res => this.Products = res as Products[]), error => console.log(error));
    this.addForm.reset();

  }

  AddProductOneValue(id: number) {
    this.service.addProductOneValue(id).subscribe(_ => this.service.getProducts().subscribe(res => this.Products = res as Products[]));
  }

  DropProductOneValue(id: number) {
    this.service.dropProductOneValue(id).subscribe(_ => this.service.getProducts().subscribe(res => this.Products = res as Products[]));
  }

  ChangeCount(event: any) {
    this.countValue = event.target.value;
  }

  exportExcel(): void {
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.Products);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
    XLSX.writeFile(wb, this.fileName);
  }
}
