import { Component, Input, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core'
import { MapDefaults } from '../../../map/models/mapDefaults'
import { Event, EventInstance } from '../../../../reducers/event/event.model'
import { AppState } from '../../../../reducers'
import { Store, select } from '@ngrx/store'
import { MapPin } from '../../../map/models/mapPin'
import {
    ShowEvents,
    SelectActiveEvent,
} from '../../../../reducers/event/event.actions'
import { spinnerColors } from '../../../../core/spinnerColors'
import { activeEventSelector } from '../../../../reducers/event/event.selectors'
import { Subscription } from 'rxjs'
import { responsesSelector } from '../../../../reducers/response/response.selectors'
import { Response, ResponseState } from '../../../../reducers/response/response.model'
import { SelectActiveResponse } from '../../../../reducers/response/response.actions'

@Component({
    selector: 'app-event-card',
    templateUrl: './event-card.component.html',
    styleUrls: [ './event-card.component.scss' ],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EventCardComponent implements OnInit, OnDestroy {
    mapOptions: MapDefaults
    pins: MapPin[]
    mapVisible = false
    devices: MapPin[]
    eventEvents: any[]
    eventEventsCount: number
    circleColor = spinnerColors.activeCircleColor
    spinnerColor = spinnerColors.activeSpinnerColor
    active = false
    response: Response

    latestEventInstance: EventInstance;

    @Input()
    event: Event

    private responsesSub$: Subscription
    private activeEventSub$: Subscription

    constructor (private store: Store<AppState>) { }

    ngOnInit() {
        this.mapOptions = {
            mapId: this.event.eventClusterId,
            height: '100px',
            padding: 10,
            useHtmlLayer: false,
            zoom: 15,
        }

        this.latestEventInstance = this.event.events[ 0 ];

        this.eventEvents = this.event.events.slice(0, 3)
        this.eventEventsCount = this.event.eventCount

        this.setupPin()

        this.responsesSub$ = this.store.pipe(select(responsesSelector)).subscribe(responses => {
            this.response = responses.find(
                response => response.primaryEventClusterId === this.event.eventClusterId
            )

            if (this.response) {
                switch (this.response.color.toLowerCase()) {
                    case 'red':
                        this.spinnerColor = spinnerColors.redSpinnerColor;
                        this.circleColor = spinnerColors.redCircleColor;
                        break;
                    case 'blue':
                        this.spinnerColor = spinnerColors.blueSpinnerColor;
                        this.circleColor = spinnerColors.blueCircleColor;
                        break;
                    case 'yellow':
                        this.spinnerColor = spinnerColors.yellowSpinnerColor;
                        this.circleColor = spinnerColors.yellowCircleColor;
                        break;
                }
                if (this.response.responseState === ResponseState.Inactive) {
                    this.spinnerColor = spinnerColors.greenSpinnerColor;
                    this.circleColor = spinnerColors.greenCircleColor;
                }
            }
        })
    }

    getResponseColor() {
        return this.response ? this.response.responseState === ResponseState.Inactive ? 'green' : this.response.color.toLowerCase() : ''
    }

    ngOnDestroy() {
        this.activeEventSub$ && this.activeEventSub$.unsubscribe()
        this.responsesSub$.unsubscribe()
    }

    setupPin() {
        this.pins = [
            {
                ...this.event.device,
                event: this.event,
            },
        ]
    }

    showEvent = () => {
        this.store.dispatch(new ShowEvents({ events: [ this.event ] }))
    }

    getBorderStyle = () => {
        if (this.active) {
            return {
                border: '1px solid #3322FF',
            }
        }

        return ''
    }

    activateResponse = () => {
        this.activeEventSub$ = this.store
            .pipe(select(activeEventSelector))
            .subscribe(activeEvent => {
                if (
                    activeEvent &&
                    activeEvent.eventClusterId === this.event.eventClusterId
                ) {
                    this.active = true
                } else if (this.activeEventSub$) {
                    this.active = false
                    this.activeEventSub$.unsubscribe()
                }
            })
        this.store.dispatch(new SelectActiveEvent({ event: this.event }))
    }

    manageResponse = () => {
        this.store.dispatch(new SelectActiveResponse({ response: this.response }))
    }

    toggleMapVisibility = () => {
        this.mapVisible = !this.mapVisible
    }
}
