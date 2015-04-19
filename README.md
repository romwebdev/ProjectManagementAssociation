# ProjectManagementAssociation

ASP.NET MVC : Gestion des élèves pour association
(Entity Framework - code first)

Le projet comprend un ensemble de fonctionnalité
  - identity 2.1 (rôles - user)
       - Install-Package Microsoft.AspNet.Identity.Samples -Pre
            - gestion des rôles (personnalisée)
            - gestion des users (personnalisée)
  - gestion des élèves 
  - gestion relation elève - parents
  - utilisation de bootstrap 3 (multiple - modal, ajax)
  - pagination ajax (pagedlist.mvc)

Prochainement :
  - fonctionnalité dbContext insertion via script SQL file et/ou procédure stockée
  - Développement de la gestion des inscriptions
        - modal, système de filtre, génération document (pdf/docx). 
  - Développement de la gestion des cours (avec historique, plus des catégories)
