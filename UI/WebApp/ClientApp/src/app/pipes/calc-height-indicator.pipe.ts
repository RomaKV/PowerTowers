import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'CalcHeightIndicator'
})
export class CalcHeightIndicatorPipe implements PipeTransform {
  transform(value: number, maxHeight: number): string {

    var result =  (maxHeight / 100) * (100 - value);
      return Math.ceil(result) + 'px';
    }

}
