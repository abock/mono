/**
 * Namespace: System.Web.UI.WebControls
 * Class:     Style
 *
 * Author:  Gaurav Vaish
 * Maintainer: gvaish@iitk.ac.in
 * Contact: <my_scripts2001@yahoo.com>, <gvaish@iitk.ac.in>
 * Implementation: yes
 * Status:  100%
 *
 * (C) Gaurav Vaish (2001)
 */

using System;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace System.Web.UI.WebControls
{
	public class Style : Component , IStateManager
	{
		internal static int MARKED      = (0x01 << 0);
		internal static int BACKCOLOR   = (0x01 << 1);
		internal static int BORDERCOLOR = (0x01 << 2);
		internal static int BORDERSTYLE = (0x01 << 3);
		internal static int BORDERWIDTH = (0x01 << 4);
		internal static int CSSCLASS    = (0x01 << 5);
		internal static int FORECOLOR   = (0x01 << 6);
		internal static int HEIGHT      = (0x01 << 7);
		internal static int WIDTH       = (0x01 << 8);
		internal static int FONT_BOLD   = (0x01 << 9);
		internal static int FONT_ITALIC = (0x01 << 10);
		internal static int FONT_NAMES  = (0x01 << 11);
		internal static int FONT_SIZE   = (0x01 << 12);
		internal static int FONT_STRIKE = (0x01 << 13);
		internal static int FONT_OLINE  = (0x01 << 14);
		internal static int FONT_ULINE  = (0x01 << 15);

		internal static string selectionBitString = "_SystemWebUIWebControlsStyle_SBS";

		private StateBag viewState;
		private int      selectionBits;
		private bool     selfStateBag;

		private FontInfo font;

		public Style()
		{
			Initialize(null);
			selfStateBag = true;
		}

		public Style(StateBag bag): base()
		{
			Initialize(bag);
			selfStateBag = false;
		}

		private void Initialize(StateBag bag)
		{
			viewState     = bag;
			selectionBits = 0x00;
		}

		internal virtual StateBag ViewState
		{
			get
			{
				if(viewState == null)
				{
					viewState = new StateBag(false);
					if(IsTrackingViewState)
						viewState.TrackViewState();
				}
				return viewState;
			}
		}

		internal bool IsSet(int bit)
		{
			return ( (selectionBits & bit) != 0x00);
		}

		internal virtual void Set(int bit)
		{
			selectionBits |= bit;
			if(IsTrackingViewState)
				selectionBits |= MARKED;
		}

		public Color BackColor
		{
			get
			{
				if(IsSet(BACKCOLOR))
					return (Color)ViewState["BackColor"];
				return Color.Empty;
			}
			set
			{
				ViewState["BackColor"] = value;
				Set(BACKCOLOR);
			}
		}

		public Color BorderColor
		{
			get
			{
				if(IsSet(BORDERCOLOR))
					return (Color)ViewState["BorderColor"];
				return Color.Empty;
			}
			set
			{
				ViewState["BorderColor"] = value;
				Set(BORDERCOLOR);
			}
		}

		public BorderStyle BorderStyle
		{
			get
			{
				if(IsSet(BORDERSTYLE))
					return (BorderStyle)ViewState["BorderStyle"];
				return BorderStyle.NotSet;
			}
			set
			{
				ViewState["BorderStyle"] = value;
				Set(BORDERSTYLE);
			}
		}

		public Unit BorderWidth
		{
			get
			{
				if(IsSet(BORDERWIDTH))
					return (Unit)ViewState["BorderWidth"];
				return Unit.Empty;
			}
			set
			{
				ViewState["BorderWidth"] = value;
				Set(BORDERWIDTH);
			}
		}

		public string CssClass
		{
			get
			{
				if(IsSet(CSSCLASS))
					return (string)ViewState["CssClass"];
				return string.Empty;
			}
			set
			{
				ViewState["CssClass"] = value;
				Set(CSSCLASS);
			}
		}

		public Color ForeColor
		{
			get
			{
				if(IsSet(FORECOLOR))
					return (Color)ViewState["ForeColor"];
				return Color.Empty;
			}
			set
			{
				ViewState["ForeColor"] = value;
				Set(FORECOLOR);
			}
		}

		public Unit Height
		{
			get
			{
				if(IsSet(HEIGHT))
					return (Unit)ViewState["Height"];
				return Unit.Empty;
			}
			set
			{
				ViewState["Height"] = value;
				Set(HEIGHT);
			}
		}

		public Unit Width
		{
			get
			{
				if(IsSet(WIDTH))
					return (Unit)ViewState["Width"];
				return Unit.Empty;
			}
			set
			{
				ViewState["Width"] = value;
				Set(HEIGHT);
			}
		}

		public FontInfo Font
		{
			get
			{
				if(font==null)
					font = new FontInfo(this);
				return font;
			}
		}

		internal virtual bool IsEmpty
		{
			get
			{
				return (selectionBits != 0);
			}
		}

		private void AddColor(HtmlTextWriter writer, HtmlTextWriterStyle style, Color color)
		{
			if(!color.IsEmpty)
				writer.AddStyleAttribute(style, ColorTranslator.ToHtml(color));
		}

		private static string StringArrayToString(string[] array, char separator)
		{
			if(array.Length == 0)
				return String.Empty;
			StringBuilder sb = new StringBuilder();
			for(int i=0; i < array.Length; i++)
			{
				if(i==0)
				{
					sb.Append(array[0]);
				} else
				{
					sb.Append(separator);
					sb.Append(array[i]);
				}
			}
			return sb.ToString();
		}

		public void AddAttributesToRender(HtmlTextWriter writer)
		{
			AddAttributesToRender(writer, null);
		}

		public void AddAttributesToRender(HtmlTextWriter writer, WebControl owner)
		{
			if(IsSet(BACKCOLOR))
			{
				AddColor(writer, HtmlTextWriterStyle.BackgroundColor, (Color)ViewState["BackColor"]);
			}

			if(IsSet(BORDERCOLOR))
			{
				AddColor(writer, HtmlTextWriterStyle.BorderColor, (Color)ViewState["BorderColor"]);
			}

			if(IsSet(FORECOLOR))
			{
				AddColor(writer, HtmlTextWriterStyle.Color, (Color)ViewState["ForeColor"]);
			}

			if(IsSet(CSSCLASS))
			{
				string cssClass = (string)ViewState["CssClass"];
				if(cssClass.Length > 0)
					writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
			}

			if(!BorderWidth.IsEmpty)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, BorderWidth.ToString(CultureInfo.InvariantCulture));
				if(BorderStyle!=BorderStyle.NotSet)
				{
					writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, Enum.Format(typeof(BorderStyle), BorderStyle, "G"));
				} else
				{
					if(BorderWidth.Value != 0.0)
					{
						writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
					}
				}
			} else
			{
				if(BorderStyle!=BorderStyle.NotSet)
				{
					writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, Enum.Format(typeof(BorderStyle), BorderStyle, "G"));
				}
			}

			if(Font.Names.Length > 0)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, StringArrayToString(Font.Names,','));
			}

			if(!Font.Size.IsEmpty)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, Font.Size.ToString(CultureInfo.InvariantCulture));
			}

			if(Font.Bold)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "bold");
			}

			if(Font.Italic)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.FontStyle, "italic");
			}

			string textDecoration = String.Empty;
			if(Font.Strikeout)
			{
				textDecoration += " strikeout";
			}
			if(Font.Underline)
			{
				textDecoration += " underline";
			}
			if(Font.Overline)
			{
				textDecoration += " overline";
			}
			if(textDecoration.Length > 0)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.TextDecoration, textDecoration);
			}

			Unit u = Unit.Empty;
			if(IsSet(HEIGHT))
			{
				u = (Unit)ViewState["Height"];
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, u.ToString(CultureInfo.InvariantCulture));
			}
			if(IsSet(WIDTH))
			{
				u = (Unit)ViewState["Width"];
				writer.AddStyleAttribute(HtmlTextWriterStyle.Width, u.ToString(CultureInfo.InvariantCulture));
			}
		}

		public virtual void CopyFrom(Style source)
		{
			if(source!=null && !source.IsEmpty)
			{
				Font.CopyFrom(source.Font);
				if(source.Height!=Unit.Empty)
				{
					Height = source.Height;
				}
				if(source.Width!=Unit.Empty)
				{
					Width = source.Width;
				}
				if(source.BorderColor!=Color.Empty)
				{
					BorderColor = source.BorderColor;
				}
				if(source.BorderWidth!=Unit.Empty)
				{
					BorderWidth = source.BorderWidth;
				}
				if(source.BorderStyle!=BorderStyle.NotSet)
				{
					BorderStyle = source.BorderStyle;
				}
				if(source.BackColor!=Color.Empty)
				{
					BackColor = source.BackColor;
				}
				if(source.CssClass!=String.Empty)
				{
					CssClass = source.CssClass;
				}
				if(source.ForeColor!=Color.Empty)
				{
					ForeColor = source.ForeColor;
				}
			}
		}

		public virtual void MergeWith(Style with)
		{
			if(with!=null && !with.IsEmpty)
			{
				if(IsEmpty)
				{
					CopyFrom(with);
					return;
				}

				Font.MergeWith(with.Font);
				if(!IsSet(HEIGHT) && with.Height!=Unit.Empty)
				{
					Height = with.Height;
				}
				if(!IsSet(WIDTH) && with.Width!=Unit.Empty)
				{
					Width = with.Width;
				}
				if(!IsSet(BORDERCOLOR) && with.BorderColor!=Color.Empty)
				{
					BorderColor = with.BorderColor;
				}
				if(!IsSet(BORDERWIDTH) && with.BorderWidth!=Unit.Empty)
				{
					BorderWidth = with.BorderWidth;
				}
				if(!IsSet(BORDERSTYLE) && with.BorderStyle!=BorderStyle.NotSet)
				{
					BorderStyle = with.BorderStyle;
				}
				if(!IsSet(BACKCOLOR) && with.BackColor!=Color.Empty)
				{
					BackColor = with.BackColor;
				}
				if(!IsSet(CSSCLASS) && with.CssClass!=String.Empty)
				{
					CssClass = with.CssClass;
				}
				if(!IsSet(FORECOLOR) && with.ForeColor!=Color.Empty)
				{
					ForeColor = with.ForeColor;
				}
			}
		}
		
		public virtual void Reset()
		{
			if(IsSet(BACKCOLOR))
				ViewState.Remove("BackColor");
			if(IsSet(BORDERCOLOR))
				ViewState.Remove("BorderColor");
			if(IsSet(BORDERSTYLE))
				ViewState.Remove("BorderStyle");
			if(IsSet(BORDERWIDTH))
				ViewState.Remove("BorderWidth");
			if(IsSet(CSSCLASS))
				ViewState.Remove("CssClass");
			if(IsSet(FORECOLOR))
				ViewState.Remove("ForeColor");
			if(IsSet(HEIGHT))
				ViewState.Remove("Height");
			if(IsSet(WIDTH))
				ViewState.Remove("Width");
			if(font!=null)
				font.Reset();
			selectionBits = 0x00;
		}

		protected bool IsTrackingViewState
		{
			get
			{
				return ( (selectionBits & MARKED) != 0x00 );
			}
		}

		protected internal virtual void TrackViewState()
		{
			if(viewState!=null)
				ViewState.TrackViewState();
			Set(MARKED);
		}

		protected internal object SaveViewState()
		{
			if(viewState != null)
			{
				if(IsSet(MARKED))
				{
					ViewState[selectionBitString] = selectionBits;
				}
				if(selfStateBag)
				{
					return ViewState.SaveViewState();
				}
			}
			return null;
		}
		
		protected internal void LoadViewState(object state)
		{
			if(state!=null && selfStateBag)
			{
				ViewState.LoadViewState(state);
			}
			if(viewState!=null)
			{
				selectionBits = (int)ViewState[selectionBitString];
			}
		}
		
		void IStateManager.LoadViewState(object state)
		{
			LoadViewState(state);
		}
		
		object IStateManager.SaveViewState()
		{
			return SaveViewState();
		}
		
		void IStateManager.TrackViewState()
		{
			TrackViewState();
		}
		
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return IsTrackingViewState;
			}
		}
	}
}
