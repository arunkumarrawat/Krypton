﻿// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2012. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 17/267 Nepean Hwy, 
//  Seaford, Vic 3198, Australia and are supplied subject to licence terms.
// 
//  Version 4.4.1.0 	www.ComponentFactory.com
// *****************************************************************************

using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using ComponentFactory.Krypton.Toolkit;

namespace ComponentFactory.Krypton.Ribbon
{
    /// <summary>
    /// Generate click events when the recent document is pressed.
    /// </summary>
    internal class RecentDocController : GlobalId,
                                         IMouseController,
                                         IKeyController,
                                         ISourceController,
                                         IContextMenuTarget
	{
		#region Instance Fields
        private bool _mouseOver;
        private ViewDrawRibbonAppMenuRecentDec _menuItem;
		private NeedPaintHandler _needPaint;
        private ViewContextMenuManager _viewManager;
		#endregion

		#region Identity
		/// <summary>
        /// Initialize a new instance of the RecentDocController class.
		/// </summary>
        /// <param name="viewManager">Owning view manager instance.</param>
        /// <param name="menuItem">Target menu item view element.</param>
        /// <param name="needPaint">Delegate for notifying paint requests.</param>
        public RecentDocController(ViewContextMenuManager viewManager,
                                   ViewDrawRibbonAppMenuRecentDec menuItem,
                                   NeedPaintHandler needPaint)
		{
            Debug.Assert(viewManager != null);
            Debug.Assert(menuItem != null);
            Debug.Assert(needPaint != null);

            _viewManager = viewManager;
            _menuItem = menuItem;
            NeedPaint = needPaint;
        }
		#endregion

        #region ContextMenuTarget Notifications
        /// <summary>
        /// Returns if the item shows a sub menu when selected.
        /// </summary>
        public virtual bool HasSubMenu
        {
            get { return false; }
        }

        /// <summary>
        /// This target should display as the active target.
        /// </summary>
        public virtual void ShowTarget()
        {
            HighlightState();
        }

        /// <summary>
        /// This target should clear any active display.
        /// </summary>
        public virtual void ClearTarget()
        {
            NormalState();
        }

        /// <summary>
        /// This target should show any appropriate sub menu.
        /// </summary>
        public void ShowSubMenu()
        {
        }

        /// <summary>
        /// This target should remove any showing sub menu.
        /// </summary>
        public void ClearSubMenu()
        {
        }

        /// <summary>
        /// Determine if the keys value matches the mnemonic setting for this target.
        /// </summary>
        /// <param name="charCode">Key code to test against.</param>
        /// <returns>True if a match is found; otherwise false.</returns>
        public bool MatchMnemonic(char charCode)
        {
            return Control.IsMnemonic(charCode, _menuItem.ShortcutText);
        }

        /// <summary>
        /// Activate the item because of a mnemonic key press.
        /// </summary>
        public void MnemonicActivate()
        {
            PressMenuItem();
        }

        /// <summary>
        /// Gets the view element that should be used when this target is active.
        /// </summary>
        /// <returns>View element to become active.</returns>
        public ViewBase GetActiveView()
        {
            return _menuItem;
        }

        /// <summary>
        /// Get the client rectangle for the display of this target.
        /// </summary>
        public Rectangle ClientRectangle 
        {
            get { return _menuItem.ClientRectangle; }
        }

        /// <summary>
        /// Should a mouse down at the provided point cause the currently stacked context menu to become current.
        /// </summary>
        /// <param name="pt">Client coordinates point.</param>
        /// <returns>True to become current; otherwise false.</returns>
        public bool DoesStackedClientMouseDownBecomeCurrent(Point pt)
        {
            return true;
        }
        #endregion

        #region Mouse Notifications
        /// <summary>
		/// Mouse has entered the view.
		/// </summary>
        /// <param name="c">Reference to the source control instance.</param>
        public virtual void MouseEnter(Control c)
		{
            if (!_mouseOver)
            {
                _mouseOver = true;
                ViewManager.SetTarget(this, true);
            }
		}

		/// <summary>
		/// Mouse has moved inside the view.
		/// </summary>
        /// <param name="c">Reference to the source control instance.</param>
        /// <param name="pt">Mouse position relative to control.</param>
        public virtual void MouseMove(Control c, Point pt)
		{
		}

		/// <summary>
		/// Mouse button has been pressed in the view.
		/// </summary>
        /// <param name="c">Reference to the source control instance.</param>
        /// <param name="pt">Mouse position relative to control.</param>
		/// <param name="button">Mouse button pressed down.</param>
		/// <returns>True if capturing input; otherwise false.</returns>
        public virtual bool MouseDown(Control c, Point pt, MouseButtons button)
		{
			return false;
		}

		/// <summary>
		/// Mouse button has been released in the view.
		/// </summary>
        /// <param name="c">Reference to the source control instance.</param>
        /// <param name="pt">Mouse position relative to control.</param>
		/// <param name="button">Mouse button released.</param>
        public virtual void MouseUp(Control c, Point pt, MouseButtons button)
		{
            PressMenuItem();
		}

		/// <summary>
		/// Mouse has left the view.
		/// </summary>
        /// <param name="c">Reference to the source control instance.</param>
        /// <param name="next">Reference to view that is next to have the mouse.</param>
        public virtual void MouseLeave(Control c, ViewBase next)
		{
            // Only if mouse is leaving all the children monitored by controller.
            if (_mouseOver && !_menuItem.ContainsRecurse(next))
            {
                _mouseOver = false;
                ViewManager.ClearTarget(this);
            }
        }

        /// <summary>
        /// Left mouse button double click.
        /// </summary>
        /// <param name="pt">Mouse position relative to control.</param>
        public virtual void DoubleClick(Point pt)
        {
        }

        /// <summary>
        /// Should the left mouse down be ignored when present on a visual form border area.
        /// </summary>
        public virtual bool IgnoreVisualFormLeftButtonDown
        {
            get { return false; }
        }
        #endregion

        #region Key Notifications
        /// <summary>
        /// Key has been pressed down.
        /// </summary>
        /// <param name="c">Reference to the source control instance.</param>
        /// <param name="e">A KeyEventArgs that contains the event data.</param>
        public virtual void KeyDown(Control c, KeyEventArgs e)
        {
            Debug.Assert(c != null);
            Debug.Assert(e != null);

            // Validate incoming references
            if (c == null)  throw new ArgumentNullException("c");
            if (e == null)  throw new ArgumentNullException("e");

            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Space:
                    PressMenuItem();
                    break;
                case Keys.Tab:
                    _viewManager.KeyTab(e.Shift);
                    break;
                case Keys.Home:
                    _viewManager.KeyHome();
                    break;
                case Keys.End:
                    _viewManager.KeyEnd();
                    break;
                case Keys.Up:
                    _viewManager.KeyUp();
                    break;
                case Keys.Down:
                    _viewManager.KeyDown();
                    break;
                case Keys.Left:
                    _viewManager.KeyLeft(true);
                    break;
                case Keys.Right:
                    _viewManager.KeyRight();
                    break;
            }
        }

        /// <summary>
        /// Key has been pressed.
        /// </summary>
        /// <param name="c">Reference to the source control instance.</param>
        /// <param name="e">A KeyPressEventArgs that contains the event data.</param>
        public virtual void KeyPress(Control c, KeyPressEventArgs e)
        {
            Debug.Assert(c != null);
            Debug.Assert(e != null);

            // Validate incoming references
            if (c == null) throw new ArgumentNullException("c");
            if (e == null) throw new ArgumentNullException("e");

            _viewManager.KeyMnemonic(e.KeyChar);
        }

        /// <summary>
        /// Key has been released.
        /// </summary>
        /// <param name="c">Reference to the source control instance.</param>
        /// <param name="e">A KeyEventArgs that contains the event data.</param>
        /// <returns>True if capturing input; otherwise false.</returns>
        public virtual bool KeyUp(Control c, KeyEventArgs e)
        {
            Debug.Assert(c != null);
            Debug.Assert(e != null);

            // Validate incoming references
            if (c == null) throw new ArgumentNullException("c");
            if (e == null) throw new ArgumentNullException("e");

            return false;
        }
        #endregion

        #region Source Notifications
        /// <summary>
        /// Source control has lost the focus.
        /// </summary>
        /// <param name="c">Reference to the source control instance.</param>
        public virtual void GotFocus(Control c)
        {
        }

        /// <summary>
        /// Source control has lost the focus.
        /// </summary>
        /// <param name="c">Reference to the source control instance.</param>
        public virtual void LostFocus(Control c)
        {
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the need paint delegate for notifying paint requests.
        /// </summary>
        public NeedPaintHandler NeedPaint
        {
            get { return _needPaint; }

            set
            {
                // Warn if multiple sources want to hook their single delegate
                Debug.Assert(((_needPaint == null) && (value != null)) ||
                             ((_needPaint != null) && (value == null)));

                _needPaint = value;
            }
        }

		/// <summary>
		/// Fires the NeedPaint event.
		/// </summary>
        /// <param name="layout">Does a layout need to occur.</param>
		public void PerformNeedPaint(bool layout)
		{
            OnNeedPaint(layout);
		}
		#endregion

        #region Implementation
        private ViewContextMenuManager ViewManager
        {
            get { return _viewManager; }
        }

        private void PressMenuItem()
        {
            // Is the menu capable of being closed?
            if (_menuItem.CanCloseMenu)
            {
                // Ask the original context menu definition, if we can close
                CancelEventArgs cea = new CancelEventArgs();
                _menuItem.Closing(cea);

                if (!cea.Cancel)
                {
                    // Close the menu from display and pass in the item clicked as the reason
                    _menuItem.Close(new CloseReasonEventArgs(ToolStripDropDownCloseReason.ItemClicked));
                }
            }

            // Generate a click event for the menu item
            _menuItem.RecentDoc.PerformClick();
            PerformNeedPaint(true);
        }

        private void HighlightState()
        {
            _menuItem.ElementState = PaletteState.Tracking;
            _menuItem.SetPalettes(_menuItem.Provider.ProviderStateHighlight.ItemHighlight.Back,
                                  _menuItem.Provider.ProviderStateHighlight.ItemHighlight.Border);

            PerformNeedPaint(false);
        }

        private void NormalState()
        {
            _menuItem.ElementState = PaletteState.Normal;
            _menuItem.SetPalettes(_menuItem.Provider.ProviderStateNormal.ItemHighlight.Back,
                                  _menuItem.Provider.ProviderStateNormal.ItemHighlight.Border);

            PerformNeedPaint(false);
        }

        /// <summary>
		/// Raises the NeedPaint event.
		/// </summary>
		/// <param name="needLayout">Does the palette change require a layout.</param>
		protected virtual void OnNeedPaint(bool needLayout)
		{
            if (_needPaint != null)
                _needPaint(this, new NeedLayoutEventArgs(needLayout, _menuItem.ClientRectangle));
		}
		#endregion
	}
}
