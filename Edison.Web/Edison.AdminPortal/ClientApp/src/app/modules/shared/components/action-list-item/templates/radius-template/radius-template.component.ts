import {
    Component,
    Input,
    ChangeDetectionStrategy,
    EventEmitter,
} from '@angular/core'
import {
    ActionPlanRadiusAction,
    ActionPlanColor,
} from '../../../../../../reducers/action-plan/action-plan.model'

@Component({
    selector: 'app-radius-template',
    templateUrl: './radius-template.component.html',
    styleUrls: [ './radius-template.component.scss' ],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RadiusTemplateComponent {
    @Input()
    context: ActionPlanRadiusAction

    @Input()
    last: boolean

    @Input()
    canUpdate: boolean

    @Input()
    onchange: EventEmitter<any>

    actionPlanColors = Object.keys(ActionPlanColor).map(key => ActionPlanColor[ key ])

    getBgColor() {
        return this.context.parameters.color.toLowerCase()
    }

    updateColor(color: string) {
        if (this.context.parameters.color === color) {
            return
        }

        this.context.parameters.color = color
        this.onchange.emit()
    }
}
