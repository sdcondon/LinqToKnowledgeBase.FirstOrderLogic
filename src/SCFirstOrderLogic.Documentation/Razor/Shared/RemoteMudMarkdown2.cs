using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudBlazor;

namespace SCFirstOrderLogic.Documentation.Razor.Shared
{
    public class RemoteMudMarkdown2(HttpClient httpClient) : ComponentBase
    {
        private RenderFragment? _renderFragment;

        [Parameter]
        public required string Uri { get; set; }

        [Parameter]
        public MarkdownPipeline? Pipeline { get; set; } = null;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            _renderFragment?.Invoke(builder);
        }

        protected override async Task OnInitializedAsync()
        {
            var markdown = await httpClient.GetStringAsync(Uri);
            var renderer = new MudRenderFragmentMarkdownRenderer();

            _renderFragment = (RenderFragment)Markdown.Convert(markdown, renderer, Pipeline);
        }

        /// <summary>
        /// An implementation of <see cref="IMarkdownRenderer"/> that converts markdown objects into a
        /// <see cref="RenderFragment"/> that renders an equivalent sequence of <see cref="MudComponentBase"/>
        /// instances.
        /// </summary>
        private class MudRenderFragmentMarkdownRenderer : RendererBase
        {
            private readonly List<RenderFragment> _renderFragments = new();

            public MudRenderFragmentMarkdownRenderer()
            {
                // Block renderers
                ////ObjectRenderers.Add(new CodeBlockRenderer());
                ////ObjectRenderers.Add(new ListRenderer());
                ObjectRenderers.Add(new HeadingRenderer());
                ////ObjectRenderers.Add(new HtmlBlockRenderer());
                ////ObjectRenderers.Add(new ParagraphRenderer());
                ////ObjectRenderers.Add(new QuoteBlockRenderer());
                ////ObjectRenderers.Add(new ThematicBreakRenderer());

                // Inline renderers
                ////ObjectRenderers.Add(new AutolinkInlineRenderer());
                ////ObjectRenderers.Add(new CodeInlineRenderer());
                ////ObjectRenderers.Add(new DelimiterInlineRenderer());
                ////ObjectRenderers.Add(new EmphasisInlineRenderer());
                ////ObjectRenderers.Add(new LineBreakInlineRenderer());
                ////ObjectRenderers.Add(new HtmlInlineRenderer());
                ////ObjectRenderers.Add(new HtmlEntityInlineRenderer());
                ////ObjectRenderers.Add(new LinkInlineRenderer());
                ////ObjectRenderers.Add(new LiteralInlineRenderer());
            }

            public override object Render(MarkdownObject markdownObject)
            {
                Write(markdownObject);

                return new RenderFragment(builder =>
                {
                    foreach (var renderFragment in _renderFragments)
                    {
                        renderFragment.Invoke(builder);
                    }
                });
            }

            public void AddMudText(Typo typo, Inline? firstInline)
            {
                _renderFragments.Add(builder =>
                {
                    int i = 0;
                    builder.OpenComponent(i++, typeof(MudText));
                    builder.AddComponentParameter(i++, nameof(MudText.Typo), typo);
                    builder.AddComponentParameter(i++, nameof(MudText.ChildContent), GetInlinesRenderFragment(firstInline));
                    builder.CloseComponent();
                });
            }

            private RenderFragment GetInlinesRenderFragment(Inline? firstInline)
            {
                var inlinesRenderer = new MudRenderFragmentMarkdownRenderer();

                var inline = firstInline;
                while (inline != null)
                {
                    Write(inline);
                    inline = inline.NextSibling;
                }
            }
        }

        private class InlinesRenderer : RendererBase
        {
            private readonly List<RenderFragment> _renderFragments = new();

            public InlinesRenderer()
            {
                // Inline renderers
                ////ObjectRenderers.Add(new AutolinkInlineRenderer());
                ////ObjectRenderers.Add(new CodeInlineRenderer());
                ////ObjectRenderers.Add(new DelimiterInlineRenderer());
                ////ObjectRenderers.Add(new EmphasisInlineRenderer());
                ////ObjectRenderers.Add(new LineBreakInlineRenderer());
                ////ObjectRenderers.Add(new HtmlInlineRenderer());
                ////ObjectRenderers.Add(new HtmlEntityInlineRenderer());
                ////ObjectRenderers.Add(new LinkInlineRenderer());
                ////ObjectRenderers.Add(new LiteralInlineRenderer());
            }

            public override object Render(MarkdownObject markdownObject)
            {
                Write(markdownObject);

                return new RenderFragment(builder =>
                {
                    foreach (var renderFragment in _renderFragments)
                    {
                        renderFragment.Invoke(builder);
                    }
                });
            }
        }

        private class HeadingRenderer : MarkdownObjectRenderer<MudRenderFragmentMarkdownRenderer, HeadingBlock>
        {
            protected override void Write(MudRenderFragmentMarkdownRenderer renderer, HeadingBlock obj)
            {
                var typo = obj.Level switch
                {
                    1 => Typo.h1,
                    2 => Typo.h2,
                    3 => Typo.h3,
                    4 => Typo.h4,
                    5 => Typo.h5,
                    6 => Typo.h6,
                    _ => Typo.body1
                };

                renderer.AddMudText(typo, obj.Inline);
            }
        }
    }
}
