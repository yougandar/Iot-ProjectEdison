import { Component, Input, Output, EventEmitter } from '@angular/core'
import { ActionPlanAction } from '../../../../reducers/action-plan/action-plan.model'
import { listFadeInOut } from '../../../../core/animations/listFadeInOut'
import { fadeInOutBorder } from '../../../../core/animations/fadeInOutBorder'

@Component({
  selector: 'app-action-list',
  templateUrl: './action-list.component.html',
  styleUrls: ['./action-list.component.scss'],
  animations: [listFadeInOut, fadeInOutBorder],
})
export class ActionListComponent {
  @Input()
  actions: ActionPlanAction[]
  @Input()
  canEdit: boolean = false
  @Input()
  canUpdate: boolean = false
  @Output()
  onchange = new EventEmitter()

  onItemChange() {
    this.onchange.emit()
  }
}
