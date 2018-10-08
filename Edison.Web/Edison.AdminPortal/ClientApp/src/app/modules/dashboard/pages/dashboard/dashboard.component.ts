import { Component, OnInit, ViewChild } from '@angular/core';
import { MapDefaults } from '../../../map/models/mapDefaults';
import { MapPin } from '../../../map/models/mapPin';
import { AppState } from '../../../../reducers';
import { Store, select } from '@ngrx/store';
import { MapComponent } from '../../../map/components/map/map.component';
import {
  eventsSelector,
  showEventsSelector,
} from '../../../../reducers/event/event.selectors';
import { GetDevices } from '../../../../reducers/device/device.actions';
import { GetEvents } from '../../../../reducers/event/event.actions';
import { devicesSelector } from '../../../../reducers/device/device.selectors';
import { Event } from '../../../../reducers/event/event.model';
import {
  responsesExist,
  responsesSelector,
} from '../../../../reducers/response/response.selectors';
import { Observable } from 'rxjs';
import {
  ResponseState,
  Response,
} from '../../../../reducers/response/response.model';
import { getRankingColor } from '../../../../shared/colorRank';
import { GetResponses } from '../../../../reducers/response/response.actions';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  @ViewChild(MapComponent)
  private mapComponent: MapComponent;
  private events: Event[];
  private responses: Response[];
  showResponses$: Observable<boolean>;

  defaultOptions: MapDefaults = {
    mapId: 'defaultMap',
    useHtmlLayer: true,
  };

  pins: MapPin[] = [];

  constructor(private store: Store<AppState>) {}

  ngOnInit() {
    this.store.pipe(select(devicesSelector)).subscribe(devices => {
      if (this.events && this.events.length > 0) {
        this.pins = devices.map(device => {
          const recentEvent = this.getRecentEvent(device.deviceId);
          return {
            ...device,
            events: recentEvent ? [recentEvent] : [],
          };
        });
      } else {
        this.pins = devices;
      }
    });

    this.store.pipe(select(eventsSelector)).subscribe(events => {
      this.events = events;
      if (this.pins && this.pins.length > 0) {
        this.pins = this.pins.map(pin => {
          const recentEvent = this.getRecentEvent(pin.deviceId);
          let pinColor = null;
          if (recentEvent) {
            const resp = this.responses.find(
              r => r.primaryEventClusterId === recentEvent.eventClusterId
            );
            if (resp) {
              pinColor =
                resp.responseState === ResponseState.Inactive
                  ? 'green'
                  : resp.color;
            } else if (recentEvent.closureDate !== null) {
              pinColor = 'grey';
            } else {
              pinColor = 'blue';
            }

            const radiusResp = this.responses.find(
              r =>
                r.responseState === ResponseState.Active &&
                r.eventClusterIds.some(ec => ec === recentEvent.eventClusterId)
            );
            if (radiusResp) {
              pinColor = radiusResp.color;
            }
          }
          return {
            ...pin,
            events: recentEvent ? [recentEvent] : [],
            color: pinColor,
          };
        });
      }
    });

    this.store.pipe(select(showEventsSelector)).subscribe(events => {
      if (events && events.length > 0) {
        this.mapComponent.focusEvents(events);
      }
    });

    this.store.pipe(select(responsesSelector)).subscribe(responses => {
      this.responses = responses;
      if (responses.length <= 0) {
        return;
      }
      this.pins = this.pins.map(pin => {
        if (!pin.events) {
          return pin;
        }

        const resp = responses.filter(response =>
          pin.events.some(
            event => event.eventClusterId === response.primaryEventClusterId
          )
        );
        const colors = resp.map(
          response =>
            response.responseState === ResponseState.Active
              ? response.color.toLowerCase()
              : 'green'
        );
        let primaryColor = getRankingColor(colors);

        if (this.events && this.events.length > 0) {
          const radiusResponse = responses.find(
            r =>
              r.responseState === ResponseState.Active &&
              r.eventClusterIds &&
              r.eventClusterIds.some(ec =>
                this.events.some(e => e.eventClusterId === ec)
              )
          );
          if (radiusResponse) {
            primaryColor = radiusResponse.color;
          }
        }

        return {
          ...pin,
          color: primaryColor,
        };
      });
    });

    this.showResponses$ = this.store.pipe(select(responsesExist));

    this.store.dispatch(new GetDevices());
    this.store.dispatch(new GetEvents());
    this.store.dispatch(new GetResponses());
  }

  getRecentEvent(deviceId) {
    if (this.events.length <= 0) {
      return null;
    }

    return this.events
      .filter(event => event.device.deviceId === deviceId)
      .sort((a, b) => b.startDate - a.startDate)
      .slice(0, 1)[0];
  }
}
