import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-icon-picker',
  templateUrl: './icon-picker.component.html',
  styleUrls: ['./icon-picker.component.scss'],
})
export class IconPickerComponent implements OnInit {

  form: FormGroup;
  @Output() pickIcon: EventEmitter<any> = new EventEmitter<any>();
  @Output() closePopover: EventEmitter<any> = new EventEmitter<any>();

  paginationConfig = {
    pageSize: 50,
    pageIndex: 1,
    total: 828
  };

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.buildFilter();
    this.searchedIcons = this.getIcons();
  }

  buildFilter() {
    this.form = this.formBuilder.group({
      iconName: [''],
      iconColor: [null]
    });

    this.form.valueChanges
      .pipe(
        debounceTime(350),
        distinctUntilChanged())
      .subscribe(value => {
        this.searchedIcons = this.getIcons();
      });
  }

  changePage(next: boolean) {
    this.paginationConfig.pageIndex = (next ? this.paginationConfig.pageIndex + 1 : this.paginationConfig.pageIndex - 1);
    this.searchedIcons = this.getIcons();
  }

  getIcons() {
    const start = (this.paginationConfig.pageIndex - 1) * this.paginationConfig.pageSize;
    const end = (this.paginationConfig.pageIndex - 1) * this.paginationConfig.pageSize + 48;
    return this.icons.filter(i => i.includes(this.form.controls.iconName.value)).slice(start, end)
  }


  pick(icon: any) {
    const emit = {
      name: icon,
      color: this.form.controls.iconColor.value
    };
    this.pickIcon.emit(emit);
    this.closePopover.emit();
  }

  icons = [
    'accessibility-outline', 'add-outline', 'add-circle-outline', 'airplane-outline', 'alarm-outline', 'albums-outline', 'alert-outline', 'alert-circle-outline',
    'american-football-outline', 'analytics-outline', 'aperture-outline', 'apps-outline', 'archive-outline', 'arrow-back-outline', 'arrow-back-circle-outline',
    'arrow-down-outline', 'arrow-down-circle-outline', 'arrow-forward-outline', 'arrow-forward-circle-outline', 'arrow-redo-outline', 'arrow-redo-circle-outline',
    'arrow-undo-outline', 'arrow-undo-circle-outline', 'arrow-up-outline', 'arrow-up-circle-outline', 'at-outline', 'at-circle-outline', 'attach-outline', 'backspace-outline',
    'bag-outline', 'bag-add-outline', 'bag-check-outline', 'bag-handle-outline', 'bag-remove-outline', 'balloon-outline', 'ban-outline', 'bandage-outline', 'bar-chart-outline',
    'barbell-outline', 'barcode-outline', 'baseball-outline', 'basket-outline', 'basketball-outline', 'battery-charging-outline', 'battery-dead-outline', 'battery-full-outline', 'battery-half-outline', 'beaker-outline', 'bed-outline', 'beer-outline', 'bicycle-outline',
    'bluetooth-outline', 'boat-outline', 'body-outline', 'bonfire-outline', 'book-outline', 'bookmark-outline', 'bookmarks-outline', 'bowling-ball-outline',
    'briefcase-outline', 'browsers-outline', 'brush-outline', 'bug-outline', 'build-outline', 'bulb-outline', 'bus-outline', 'business-outline', 'cafe-outline',
    'calculator-outline', 'calendar-outline', 'calendar-clear-outline', 'calendar-number-outline', 'call-outline', 'camera-outline', 'camera-reverse-outline', 'car-outline',
    'car-sport-outline', 'card-outline', 'caret-back-outline', 'caret-back-circle-outline', 'caret-down-outline', 'caret-down-circle-outline', 'caret-forward-outline',
    'caret-forward-circle-outline', 'caret-up-outline', 'caret-up-circle-outline', 'cart-outline', 'cash-outline', 'cellular-outline', 'chatbox-outline', 'chatbox-ellipses-outline',
    'chatbubble-outline', 'chatbubble-ellipses-outline', 'chatbubbles-outline', 'checkbox-outline', 'checkmark-outline', 'checkmark-circle-outline', 'checkmark-done-outline', 'checkmark-done-circle-outline', 'chevron-back-outline', 'chevron-back-circle-outline', 'chevron-down-outline',
    'chevron-down-circle-outline', 'chevron-forward-outline', 'chevron-forward-circle-outline', 'chevron-up-outline', 'chevron-up-circle-outline', 'clipboard-outline',
    'close-outline', 'close-circle-outline', 'cloud-outline', 'cloud-circle-outline', 'cloud-done-outline', 'cloud-download-outline', 'cloud-offline-outline',
    'cloud-upload-outline', 'cloudy-outline', 'cloudy-night-outline', 'code-outline', 'code-download-outline', 'code-slash-outline', 'code-working-outline', 'cog-outline',
    'color-fill-outline', 'color-filter-outline', 'color-palette-outline', 'color-wand-outline', 'compass-outline', 'construct-outline', 'contract-outline', 'contrast-outline',
    'copy-outline', 'create-outline', 'crop-outline', 'cube-outline', 'cut-outline', 'desktop-outline', 'diamond-outline', 'dice-outline', 'disc-outline', 'document-outline',
    'document-attach-outline', 'document-lock-outline', 'document-text-outline', 'documents-outline', 'download-outline', 'duplicate-outline', 'ear-outline', 'earth-outline', 'easel-outline', 'egg-outline', 'ellipse-outline', 'ellipsis-horizontal-outline',
    'ellipsis-horizontal-circle-outline', 'ellipsis-vertical-outline', 'ellipsis-vertical-circle-outline', 'enter-outline', 'exit-outline', 'expand-outline',
    'extension-puzzle-outline', 'eye-outline', 'eye-off-outline', 'eyedrop-outline', 'fast-food-outline', 'female-outline', 'file-tray-outline', 'file-tray-full-outline',
    'file-tray-stacked-outline', 'film-outline', 'filter-outline', 'filter-circle-outline', 'finger-print-outline', 'fish-outline', 'fitness-outline', 'flag-outline',
    'flame-outline', 'flash-outline', 'flash-off-outline', 'flashlight-outline', 'flask-outline', 'flower-outline', 'folder-outline', 'folder-open-outline', 'football-outline',
    'footsteps-outline', 'funnel-outline', 'game-controller-outline', 'gift-outline', 'git-branch-outline', 'git-commit-outline', 'git-compare-outline', 'git-merge-outline',
    'git-network-outline', 'git-pull-request-outline', 'glasses-outline', 'globe-outline', 'golf-outline', 'grid-outline', 'hammer-outline', 'hand-left-outline', 'hand-right-outline', 'happy-outline', 'hardware-chip-outline', 'headset-outline',
    'heart-outline', 'heart-circle-outline', 'heart-dislike-outline', 'heart-dislike-circle-outline', 'heart-half-outline', 'help-outline', 'help-buoy-outline',
    'help-circle-outline', 'home-outline', 'hourglass-outline', 'ice-cream-outline', 'id-card-outline', 'image-outline', 'images-outline', 'infinite-outline',
    'information-outline', 'information-circle-outline', 'invert-mode-outline', 'journal-outline', 'key-outline', 'keypad-outline', 'language-outline', 'laptop-outline',
    'layers-outline', 'leaf-outline', 'library-outline', 'link-outline', 'list-outline', 'list-circle-outline', 'locate-outline', 'location-outline', 'lock-closed-outline',
    'lock-open-outline', 'log-in-outline', 'log-out-outline', 'magnet-outline', 'mail-outline', 'mail-open-outline', 'mail-unread-outline', 'male-outline', 'male-female-outline',
    'man-outline', 'map-outline', 'medal-outline', 'medical-outline', 'medkit-outline', 'megaphone-outline', 'menu-outline', 'mic-outline', 'mic-circle-outline', 'mic-off-outline',
    'mic-off-circle-outline', 'moon-outline', 'move-outline', 'musical-note-outline', 'musical-notes-outline', 'navigate-outline', 'navigate-circle-outline',
    'newspaper-outline', 'notifications-outline', 'notifications-circle-outline', 'notifications-off-outline', 'notifications-off-circle-outline', 'nuclear-outline',
    'nutrition-outline', 'open-outline', 'options-outline', 'paper-plane-outline', 'partly-sunny-outline', 'pause-outline', 'pause-circle-outline', 'paw-outline', 'pencil-outline',
    'people-outline', 'people-circle-outline', 'person-outline', 'person-add-outline', 'person-circle-outline', 'person-remove-outline', 'phone-landscape-outline',
    'phone-portrait-outline', 'pie-chart-outline', 'pin-outline', 'pint-outline', 'pizza-outline', 'planet-outline', 'play-outline', 'play-back-outline', 'play-back-circle-outline',
    'play-circle-outline', 'play-forward-outline', 'play-forward-circle-outline', 'play-skip-back-outline', 'play-skip-back-circle-outline', 'play-skip-forward-outline', 'play-skip-forward-circle-outline',
    'podium-outline', 'power-outline', 'pricetag-outline', 'pricetags-outline', 'print-outline', 'prism-outline',
    'pulse-outline', 'push-outline', 'qr-code-outline', 'radio-outline', 'radio-button-off-outline', 'radio-button-on-outline', 'rainy-outline', 'reader-outline',
    'receipt-outline', 'recording-outline', 'refresh-outline', 'refresh-circle-outline', 'reload-outline', 'reload-circle-outline', 'remove-outline', 'remove-circle-outline',
    'reorder-four-outline', 'reorder-three-outline', 'reorder-two-outline', 'repeat-outline', 'resize-outline', 'restaurant-outline', 'return-down-back-outline',
    'return-down-forward-outline', 'return-up-back-outline', 'return-up-forward-outline', 'ribbon-outline', 'rocket-outline', 'rose-outline', 'sad-outline', 'save-outline',
    'scale-outline', 'scan-outline', 'scan-circle-outline', 'school-outline', 'search-outline', 'search-circle-outline', 'send-outline', 'server-outline', 'settings-outline',
    'shapes-outline', 'share-outline', 'share-social-outline', 'shield-outline', 'shield-checkmark-outline', 'shield-half-outline', 'shirt-outline', 'shuffle-outline', 'skull-outline', 'snow-outline', 'sparkles-outline',
    'speedometer-outline', 'square-outline', 'star-outline', 'star-half-outline', 'stats-chart-outline', 'stop-outline', 'stop-circle-outline', 'stopwatch-outline',
    'storefront-outline', 'subway-outline', 'sunny-outline', 'swap-horizontal-outline', 'swap-vertical-outline', 'sync-outline', 'sync-circle-outline', 'tablet-landscape-outline',
    'tablet-portrait-outline', 'telescope-outline', 'tennisball-outline', 'terminal-outline', 'text-outline', 'thermometer-outline', 'thumbs-down-outline', 'thumbs-up-outline',
    'thunderstorm-outline', 'ticket-outline', 'time-outline', 'timer-outline', 'today-outline', 'toggle-outline', 'trail-sign-outline', 'train-outline', 'transgender-outline',
    'trash-outline', 'trash-bin-outline', 'trending-down-outline', 'trending-up-outline', 'triangle-outline', 'trophy-outline', 'tv-outline', 'umbrella-outline', 'unlink-outline',
    'videocam-outline', 'videocam-off-outline', 'volume-high-outline', 'volume-low-outline', 'volume-medium-outline', 'volume-mute-outline', 'volume-off-outline', 'walk-outline', 'wallet-outline',
    'warning-outline', 'watch-outline', 'water-outline', 'wifi-outline', 'wine-outline', 'woman-outline',
    'accessibility', 'add', 'add-circle', 'airplane', 'alarm', 'albums', 'alert', 'alert-circle', 'american-football', 'analytics', 'aperture', 'apps', 'archive',
    'arrow-back', 'arrow-back-circle', 'arrow-down', 'arrow-down-circle', 'arrow-forward', 'arrow-forward-circle', 'arrow-redo', 'arrow-redo-circle', 'arrow-undo',
    'arrow-undo-circle', 'arrow-up', 'arrow-up-circle', 'at', 'at-circle', 'attach', 'backspace', 'bag', 'bag-add', 'bag-check', 'bag-handle', 'bag-remove', 'balloon', 'ban',
    'bandage', 'bar-chart', 'barbell', 'barcode', 'baseball', 'basket', 'basketball', 'battery-charging', 'battery-dead', 'battery-full', 'battery-half', 'beaker', 'bed', 'beer', 'bicycle',
    'bluetooth', 'boat', 'body', 'bonfire', 'book', 'bookmark', 'bookmarks', 'bowling-ball', 'briefcase', 'browsers', 'brush', 'bug', 'build', 'bulb', 'bus', 'business',
    'cafe', 'calculator', 'calendar', 'calendar-clear', 'calendar-number', 'call', 'camera', 'camera-reverse', 'car', 'car-sport', 'card', 'caret-back', 'caret-back-circle',
    'caret-down', 'caret-down-circle', 'caret-forward', 'caret-forward-circle', 'caret-up', 'caret-up-circle', 'cart', 'cash', 'cellular', 'chatbox', 'chatbox-ellipses', 'chatbubble',
    'chatbubble-ellipses', 'chatbubbles', 'checkbox', 'checkmark', 'checkmark-circle', 'checkmark-done', 'checkmark-done-circle', 'chevron-back', 'chevron-back-circle', 'chevron-down',
    'chevron-down-circle', 'chevron-forward', 'chevron-forward-circle', 'chevron-up', 'chevron-up-circle', 'clipboard', 'close', 'close-circle', 'cloud', 'cloud-circle',
    'cloud-done', 'cloud-download', 'cloud-offline', 'cloud-upload', 'cloudy', 'cloudy-night', 'code', 'code-download', 'code-slash', 'code-working', 'cog', 'color-fill',
    'color-filter', 'color-palette', 'color-wand', 'compass', 'construct', 'contract', 'contrast', 'copy', 'create', 'crop', 'cube', 'cut', 'desktop', 'diamond', 'dice', 'disc',
    'document', 'document-attach', 'document-lock', 'document-text', 'documents', 'download', 'duplicate', 'ear', 'earth', 'easel', 'egg', 'ellipse', 'ellipsis-horizontal',
    'ellipsis-horizontal-circle', 'ellipsis-vertical', 'ellipsis-vertical-circle', 'enter', 'exit', 'expand', 'extension-puzzle', 'eye', 'eye-off', 'eyedrop', 'fast-food',
    'female', 'file-tray', 'file-tray-full', 'file-tray-stacked', 'film', 'filter', 'filter-circle', 'finger-print', 'fish', 'fitness', 'flag', 'flame', 'flash', 'flash-off',
    'flashlight', 'flask', 'flower', 'folder', 'folder-open', 'football', 'footsteps', 'funnel', 'game-controller', 'gift', 'git-branch', 'git-commit', 'git-compare', 'git-merge',
    'git-network', 'git-pull-request', 'glasses', 'globe', 'golf', 'grid', 'hammer', 'hand-left', 'hand-right', 'happy', 'hardware-chip', 'headset',
    'heart', 'heart-circle', 'heart-dislike', 'heart-dislike-circle', 'heart-half', 'help', 'help-buoy', 'help-circle', 'home', 'hourglass', 'ice-cream', 'id-card', 'image',
    'images', 'infinite', 'information', 'information-circle', 'invert-mode', 'journal', 'key', 'keypad', 'language', 'laptop', 'layers', 'leaf', 'library', 'link', 'list',
    'list-circle', 'locate', 'location', 'lock-closed', 'lock-open', 'log-in', 'log-out', 'magnet', 'mail', 'mail-open', 'mail-unread', 'male', 'male-female', 'man', 'map', 'medal',
    'medical', 'medkit', 'megaphone', 'menu', 'mic', 'mic-circle', 'mic-off',
    'mic-off-circle', 'moon', 'move', 'musical-note', 'musical-notes', 'navigate', 'navigate-circle', 'newspaper', 'notifications', 'notifications-circle', 'notifications-off',
    'notifications-off-circle', 'nuclear', 'nutrition', 'open', 'options', 'paper-plane', 'partly-sunny', 'pause', 'pause-circle', 'paw', 'pencil', 'people', 'people-circle',
    'person', 'person-add', 'person-circle', 'person-remove', 'phone-landscape', 'phone-portrait', 'pie-chart', 'pin', 'pint', 'pizza', 'planet', 'play', 'play-back', 'play-back-circle',
    'play-circle', 'play-forward', 'play-forward-circle', 'play-skip-back', 'play-skip-back-circle', 'play-skip-forward', 'play-skip-forward-circle', 'podium', 'power', 'pricetag', 'pricetags', 'print', 'prism',
    'pulse', 'push', 'qr-code', 'radio', 'radio-button-off', 'radio-button-on', 'rainy', 'reader', 'receipt', 'recording', 'refresh', 'refresh-circle', 'reload', 'reload-circle',
    'remove', 'remove-circle', 'reorder-four', 'reorder-three', 'reorder-two', 'repeat', 'resize', 'restaurant', 'return-down-back', 'return-down-forward', 'return-up-back',
    'return-up-forward', 'ribbon', 'rocket', 'rose', 'sad', 'save', 'scale', 'scan', 'scan-circle', 'school', 'search', 'search-circle', 'send', 'server', 'settings', 'shapes',
    'share', 'share-social', 'shield', 'shield-checkmark', 'shield-half', 'shirt', 'shuffle', 'skull', 'snow', 'sparkles',
    'speedometer', 'square', 'star', 'star-half', 'stats-chart', 'stop', 'stop-circle', 'stopwatch', 'storefront', 'subway', 'sunny', 'swap-horizontal', 'swap-vertical', 'sync',
    'sync-circle', 'tablet-landscape', 'tablet-portrait', 'telescope', 'tennisball', 'terminal', 'text', 'thermometer', 'thumbs-down', 'thumbs-up', 'thunderstorm', 'ticket',
    'time', 'timer', 'today', 'toggle', 'trail-sign', 'train', 'transgender', 'trash', 'trash-bin', 'trending-down', 'trending-up', 'triangle', 'trophy', 'tv', 'umbrella', 'unlink',
    'videocam', 'videocam-off', 'volume-high', 'volume-low', 'volume-medium', 'volume-mute', 'volume-off', 'walk', 'wallet',
    'warning', 'watch', 'water', 'wifi', 'wine', 'woman'


  ];

  searchedIcons;
}
