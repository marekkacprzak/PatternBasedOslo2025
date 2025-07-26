// This is a simple source generator that generates a Deconstruct method for classes marked with [GeneratedDeconstruct]
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Generator
{
    [Generator]
    public class DeconstructGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not SyntaxReceiver receiver)
                return;

            var attributeSymbol = context.Compilation.GetTypeByMetadataName($"{nameof(GeneratorContract)}.{nameof(GeneratorContract.GeneratedDeconstructAttribute)}");
            if (attributeSymbol == null)
                return;

            foreach (var candidate in receiver.Candidates)
            {
                var model = context.Compilation.GetSemanticModel(candidate.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(candidate) as INamedTypeSymbol;
                if (symbol == null)
                    continue;

                if (!symbol.GetAttributes().Any(ad => ad.AttributeClass?.Equals(attributeSymbol, SymbolEqualityComparer.Default) == true))
                    continue;

                var constructors = symbol.Constructors.Where(c => c.DeclaredAccessibility == Accessibility.Public && c.Parameters.Length > 0).ToList();
                if (constructors.Count == 0)
                    continue;

                var sb = new StringBuilder();
                var ns = symbol.ContainingNamespace.IsGlobalNamespace ? null : symbol.ContainingNamespace.ToDisplayString();
                if (!string.IsNullOrEmpty(ns))
                {
                    sb.AppendLine($"namespace {ns}");
                    sb.AppendLine("{");
                }
                sb.AppendLine($"public partial class {symbol.Name}");
                sb.AppendLine("{");

                // Generate a Deconstruct method for each constructor based on parameter order
                foreach (var ctor in constructors)
                {
                    var ctorDecl = ctor.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as ConstructorDeclarationSyntax;
                    if (ctorDecl == null || ctorDecl.Body == null)
                        continue;
                    
                    if (ctor.Parameters.Length==1)
                        continue;

                    // Map parameter name to property/field assigned in constructor body
                    var paramToMember = new List<(IParameterSymbol param, ISymbol? member)>();
                    foreach (var param in ctor.Parameters)
                    {
                        ISymbol? member = null;
                        foreach (var stmt in ctorDecl.Body.Statements.OfType<ExpressionStatementSyntax>())
                        {
                            if (stmt.Expression is AssignmentExpressionSyntax assign)
                            {
                                if (assign.Right is IdentifierNameSyntax right && right.Identifier.Text == param.Name)
                                {
                                    // Handle both "this.Property = param" and "Property = param"
                                    string? memberName = null;
                                    if (assign.Left is MemberAccessExpressionSyntax left && left.Expression is ThisExpressionSyntax)
                                    {
                                        memberName = left.Name.Identifier.Text;
                                    }
                                    else if (assign.Left is IdentifierNameSyntax leftId)
                                    {
                                        memberName = leftId.Identifier.Text;
                                    }
                                    
                                    if (memberName != null)
                                    {
                                        member = symbol.GetMembers().FirstOrDefault(m => m.Name == memberName && (m.Kind == SymbolKind.Property || m.Kind == SymbolKind.Field));
                                        if (member != null) break;
                                    }
                                }
                            }
                        }
                        paramToMember.Add((param, member));
                    }
                    if (paramToMember.Count == 0)
                        continue; // Only generate if at least one parameter exists

                    sb.Append("    public void Deconstruct(");
                    sb.Append(string.Join(", ", paramToMember.Select(pm => $"out {pm.param.Type} {pm.param.Name}")));
                    sb.AppendLine(")");
                    sb.AppendLine("    {");
                    foreach (var pm in paramToMember)
                    {
                        if (pm.member != null)
                            sb.AppendLine($"        {pm.param.Name} = this.{pm.member.Name};");
                        else
                            sb.AppendLine($"        {pm.param.Name} = default;");
                    }
                    sb.AppendLine("    }");
                }

                sb.AppendLine("}");
                if (!string.IsNullOrEmpty(ns))
                {
                    sb.AppendLine("}");
                }

                context.AddSource($"{symbol.Name}.Deconstruct.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
            }
        }

        private class SyntaxReceiver : ISyntaxReceiver
        {
            public List<ClassDeclarationSyntax> Candidates { get; } = new();
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is ClassDeclarationSyntax cds && cds.AttributeLists.Count > 0)
                {
                    Candidates.Add(cds);
                }
            }
        }
    }
}
