using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSeed.Application.Interfaces;
using WorldSeed.Application.Interfaces.Services;
using WorldSeed.Domain.Entities.GroupRelated;

namespace WorldSeed.Persistence.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfwork;

        public GroupService(IUnitOfWork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }

        public Group CreateGroup(string groupName, long userId)
        {
            var userFromDB = _unitOfwork.Users.GetAll().FirstOrDefault(u => u.Id == userId);
            if (userFromDB == null)
            {
                return null!;
            }

            var forum = new Domain.Entities.ForumRelated.Forum
            {
                Name = $"{groupName} Forum",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var newGroup = new Group()
            {
                Name = groupName,
                Owner = userFromDB,
                Forum = forum,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            forum.Group = newGroup;

            var ownerMembership = new GroupMember
            {
                Group = newGroup,
                User = userFromDB,
                Rank = GroupRank.Owner,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _unitOfwork.Forums.Add(forum);
            _unitOfwork.Groups.Add(newGroup);
            _unitOfwork.GroupMembers.Add(ownerMembership);
            _unitOfwork.SaveChanges();

            return newGroup;
        }

        public GroupMember JoinGroup(int groupId, long userId)
        {
            var group = _unitOfwork.Groups.Get(groupId);
            var user = _unitOfwork.Users.GetAll().FirstOrDefault(u => u.Id == userId);
            if (group == null || user == null)
            {
                return null;
            }

            var existing = _unitOfwork.GroupMembers
                .Find(m => m.Group.Id == groupId && m.User.Id == userId)
                .FirstOrDefault();
            if (existing != null)
            {
                return null;
            }

            var membership = new GroupMember()
            {
                Group = group,
                User = user,
                Rank = GroupRank.Member,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _unitOfwork.GroupMembers.Add(membership);
            _unitOfwork.SaveChanges();
            return membership;
        }

        public bool LeaveGroup(int groupId, long userId)
        {
            var membership = _unitOfwork.GroupMembers
                .Find(m => m.Group.Id == groupId && m.User.Id == userId)
                .FirstOrDefault();
            if (membership == null)
            {
                return false;
            }

            _unitOfwork.GroupMembers.Remove(membership);
            _unitOfwork.SaveChanges();
            return true;
        }

        public bool UpdateGroupName(int groupId, string newName, long actorUserId)
        {
            var group = _unitOfwork.Groups.Get(groupId);
            if (group == null)
            {
                return false;
            }

            var membership = _unitOfwork.GroupMembers
                .Find(m => m.Group.Id == groupId && m.User.Id == actorUserId)
                .FirstOrDefault();

            bool canEdit = membership != null && (membership.Rank == GroupRank.Admin || membership.Rank == GroupRank.Owner);

            if (!canEdit)
            {
                return false;
            }

            group.Name = newName;
            group.UpdatedAt = DateTime.UtcNow;
            _unitOfwork.SaveChanges();
            return true;
        }

        public bool ChangeMemberRank(int groupId, long actorUserId, long targetUserId, GroupRank newRank)
        {
            var group = _unitOfwork.Groups.Get(groupId);
            if (group == null)
            {
                return false;
            }

            var actorMembership = _unitOfwork.GroupMembers
                .Find(m => m.Group.Id == groupId && m.User.Id == actorUserId)
                .FirstOrDefault();

            if (actorMembership == null || actorMembership.Rank != GroupRank.Owner)
            {
                return false;
            }

            var target = _unitOfwork.GroupMembers
                .Find(m => m.Group.Id == groupId && m.User.Id == targetUserId)
                .FirstOrDefault();
            if (target == null)
            {
                return false;
            }

            target.Rank = newRank;
            target.UpdatedAt = DateTime.UtcNow;
            _unitOfwork.SaveChanges();
            return true;
        }

        public IEnumerable<Group> GetGroupsForUser(long userId)
        {
            return _unitOfwork.GroupMembers
                .GetMembersWithGroups(userId)
                .Where(m => m.Group != null)
                .Select(m => m.Group!);
        }

        public IEnumerable<Group> GetJoinableGroups(long userId)
        {
            var joinedIds = _unitOfwork.GroupMembers
                .GetMembersWithGroups(userId)
                .Where(m => m.Group != null)
                .Select(m => m.Group!.Id)
                .ToHashSet();

            return _unitOfwork.Groups.GetAll()
                .Where(g => !joinedIds.Contains(g.Id));
        }

        public IEnumerable<GroupMember> GetGroupMembers(int groupId)
        {
            return _unitOfwork.GroupMembers.GetMembersWithUsers(groupId);
        }
    }
}
