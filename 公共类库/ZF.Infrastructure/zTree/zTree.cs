using System;
using System.Collections.Generic;
using System.Linq;

namespace ZF.Infrastructure.zTree
{
    public class Tree
    {
        public static void Ztree(zTree zTree, List<zTree> listzTrees)
        {
            foreach (var item in listzTrees)
            {
                if (item.pId == zTree.id)
                {
                    if (zTree.children == null)
                    {
                        zTree.children = new List<zTree>();
                    }
                    zTree.children.Add(item);
                }
            }
        }



        /// <summary>
        /// 自关联树
        /// </summary>
        /// <param name="zTree"></param>
        /// <param name="listzTrees"></param>
        public static void SelfTree( zTree zTree, List<zTree> listzTrees )
        {
            foreach ( var item in listzTrees )
            {
                if ( item.pId == zTree.id )
                {
                    if ( zTree.children == null )
                    {
                        zTree.children = new List<zTree>( );
                    }
                    zTree.children.Add( item );
                    //listzTrees.Remove( item);
                    SelfTree( item, listzTrees );
                }
            }
        }

        /// <summary>
        /// zTree实体
        /// </summary>
        public class zTree
        {
            public string id { get; set; }
            public string name { get; set; }

            public string pId { get; set; }

            public string type { get; set; }

            public  string subjectId { get; set; }

            public List<zTree> children { get; set; }



            /// <summary>
            /// 难度等级
            /// </summary>
            public string DLevel { get; set; }

            /// <summary>
            /// 试题分数
            /// </summary>
            public string QScoreSum { get; set; }

            /// <summary>
            /// 试卷结构明细ID
            /// </summary>
            public string PDetailId { get; set; }

            /// <summary>
            /// 试题数量
            /// </summary>
            public string QuestionCount { get; set; }

            /// <summary>
            /// 参数明细ID
            /// </summary>
            public string DetailId { get; set; }

            /// <summary>
            /// 知识点集合
            /// </summary>
            public string KIdList { get; set; }

            /// <summary>
            /// 节点图片
            /// </summary>
            public string icon { get; set; }

            /// <summary>
            /// 编号
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// 试题ID
            /// </summary>
            public string BigQuestionId { get; set; }

            /// <summary>
            /// 知识点唯一标识
            /// </summary>
            public string OneCode { get; set; }
            /// <summary>
            /// 知识点父id
            /// </summary>
            public string KnowledgePointPid { get; set; }

        }
    }
}