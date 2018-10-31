import { Component, Input, Output, EventEmitter } from '@angular/core'
import { Response } from '../../../../reducers/response/response.model'

@Component({
  selector: 'app-update-response',
  templateUrl: './update-response.component.html',
  styleUrls: ['./update-response.component.scss'],
})
export class UpdateResponseComponent {
  @Input()
  activeResponse: Response

  @Output()
  cancel = new EventEmitter()

  updateSucceeded = false
  modified = false

  onCancel() {
    this.cancel.emit()
  }

  onUpdate() {
    if (!this.modified) {
      return
    }

    this.modified = false
    this.updateSucceeded = true
  }

  updated() {
    this.modified = true
  }
}
