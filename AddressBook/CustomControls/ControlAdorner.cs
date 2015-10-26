using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace AddressBook.CustomControls
{
    public class ControlAdorner : Adorner
    {
        private readonly FrameworkElement _adoriningElement;

        public ControlAdorner(UIElement adornedElement, FrameworkElement adoriningElement) : base(adornedElement)
        {
            _adoriningElement = adoriningElement;
            AddVisualChild(adoriningElement);
        }

        public void RemoveChild()
        {
            RemoveVisualChild(_adoriningElement);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var adornedElementRect = new Rect(AdornedElement.RenderSize);
            _adoriningElement.Arrange(adornedElementRect);

            return finalSize;
        }

        protected override int VisualChildrenCount
        {
            get { return _adoriningElement == null ? 0 : 1; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0 && _adoriningElement != null)
                return _adoriningElement;

            return base.GetVisualChild(index);
        }
    }
}
